using AutoMapper;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs.Employee;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class EmployeeService : AuditableService, IEmployeeService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Employee> _employeeRepository;

    public EmployeeService(IMapper mapper, IRepository<Employee> employeeRepository, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _employeeRepository = employeeRepository;
    }

    public async ValueTask<EmployeeResultDto> AddAsync(EmployeeCreationDto dto)
    {
        var existingEmployee = await _employeeRepository.GetAsync(u =>
            (u.Username == dto.Username || u.UserId == dto.UserId) && !u.IsDeleted);
        if (existingEmployee is not null)
            throw new AlreadyExistException($"This Employee already exists with Username = {dto.Username}");

        dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var employee = _mapper.Map<Employee>(dto);
        SetCreatedFields(employee); // Auditable maydonlarni qo‘shish

        await _employeeRepository.CreateAsync(employee);
        await _employeeRepository.SaveChanges();

        return _mapper.Map<EmployeeResultDto>(employee);
    }

    public async ValueTask<bool> AnyEmployeesAsync()
    {
        return await _employeeRepository.GetAll().AnyAsync();
    }

    public async Task<long> CreateSeedEmployeeAsync(Employee employee)
    {
        var createdEmployee = _employeeRepository.CreateAsync(employee);
        await _employeeRepository.SaveChanges();
        return createdEmployee.Id;
    }

    public async ValueTask<EmployeeResultDto> ModifyAsync(EmployeeUpdateDto dto)
    {
        var existingEmployee = await _employeeRepository.GetAsync(u => u.Id == dto.Id && !u.IsDeleted)
            ?? throw new NotFoundException($"This Employee is not found with ID = {dto.Id}");

        if (!string.IsNullOrWhiteSpace(dto.Password))
            dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        else
            dto.Password = existingEmployee.Password;

        _mapper.Map(dto, existingEmployee);
        SetUpdatedFields(existingEmployee); // Auditable maydonlarni yangilash

        _employeeRepository.Update(existingEmployee);
        await _employeeRepository.SaveChanges();

        return _mapper.Map<EmployeeResultDto>(existingEmployee);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existingEmployee = await _employeeRepository.GetAsync(u => u.Id == id && !u.IsDeleted)
            ?? throw new NotFoundException($"This Employee is not found with ID = {id}");

        existingEmployee.IsDeleted = true; // Soft delete
        SetUpdatedFields(existingEmployee); // Auditable maydonlarni yangilash

        _employeeRepository.Update(existingEmployee);
        await _employeeRepository.SaveChanges();
        return true;
    }

    public async ValueTask<IEnumerable<EmployeeResultDto>> RetrieveAllAsync()
    {
        var branchId = GetCurrentBranchId();
        var employees = await _employeeRepository.GetAll(u => !u.IsDeleted && u.BranchId.Equals(branchId))
            .ToListAsync();
        return _mapper.Map<IEnumerable<EmployeeResultDto>>(employees);
    }

    public async ValueTask<EmployeeResultDto> RetrieveByIdAsync(long id)
    {
        var branchId = GetCurrentBranchId();
        var employee = await _employeeRepository.GetAsync(u => u.Id == id && !u.IsDeleted && u.BranchId.Equals(branchId))
            ?? throw new NotFoundException($"This Employee is not found with ID = {id}");

        return _mapper.Map<EmployeeResultDto>(employee);
    }

    public async ValueTask<Employee> VerifyEmployeeAsync(string username, string password)
    {
        var branchId = GetCurrentBranchId();
        var employee = await _employeeRepository.GetAsync(u => u.Username == username && !u.IsDeleted && u.BranchId.Equals(branchId))
            ?? throw new NotFoundException($"Employee with username {username} not found");

        if (!BCrypt.Net.BCrypt.Verify(password, employee.Password))
            throw new Exception("Incorrect password"); // UnauthorizedException o‘rniga umumiy Exception

        return employee;
    }
}