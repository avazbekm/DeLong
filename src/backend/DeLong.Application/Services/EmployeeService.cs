using BCrypt.Net;
using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using DeLong.Service.DTOs.Employee;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Employee> _employeeRepository;

    public EmployeeService(IMapper mapper, IRepository<Employee> employeeRepository)
    {
        _mapper = mapper;
        _employeeRepository = employeeRepository;
    }

    public async ValueTask<EmployeeResultDto> AddAsync(EmployeeCreationDto dto)
    {
        var existingEmployee = await _employeeRepository.GetAsync(u =>
        u.Username == dto.Username ||
        u.UserId == dto.UserId);
        if (existingEmployee is not null)
            throw new AlreadyExistException($"This Employee already exists with Username = {dto.Username}");

        // Parolni hashlash
        dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var employee = _mapper.Map<Employee>(dto);
        await _employeeRepository.CreateAsync(employee);
        await _employeeRepository.SaveChanges();

        return _mapper.Map<EmployeeResultDto>(employee);
    }

    public async ValueTask<EmployeeResultDto> ModifyAsync(EmployeeUpdateDto dto)
    {
        var existingEmployee = await _employeeRepository.GetAsync(u => u.Id == dto.Id)
            ?? throw new NotFoundException($"This Employee is not found with ID = {dto.Id}");

        // Parol yangilansa, hashlash; bo'lmasa eski qiymatni saqlash
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }
        else
        {
            dto.Password = existingEmployee.Password;
        }

        _mapper.Map(dto, existingEmployee);
        _employeeRepository.Update(existingEmployee);
        await _employeeRepository.SaveChanges();

        return _mapper.Map<EmployeeResultDto>(existingEmployee);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existingEmployee = await _employeeRepository.GetAsync(u => u.Id == id)
            ?? throw new NotFoundException($"This Employee is not found with ID = {id}");

        _employeeRepository.Delete(existingEmployee);
        await _employeeRepository.SaveChanges();
        return true;
    }

    public async ValueTask<IEnumerable<EmployeeResultDto>> RetrieveAllAsync()
    {
        var employees = await _employeeRepository.GetAll().ToListAsync();
        return _mapper.Map<IEnumerable<EmployeeResultDto>>(employees);
    }

    public async ValueTask<EmployeeResultDto> RetrieveByIdAsync(long id)
    {
        var employee = await _employeeRepository.GetAsync(u => u.Id == id)
         ?? throw new NotFoundException($"This Employee is not found with ID = {id}");

        return _mapper.Map<EmployeeResultDto>(employee);
    }
}
