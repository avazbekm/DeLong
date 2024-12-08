using DeLong.Service.Services;
using DeLong.Service.Interfaces;
using DeLong.Application.Mappers;
using DeLong.Application.Interfaces;
using DeLong.Infrastructure.Repositories;

namespace DeLong.WebAPI.Extentions;

public static class ServicesColletion
{
    public static void AddServices(this IServiceCollection services)
    {

        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICashRegisterService, CashRegisterService>();
        

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddAutoMapper(typeof(MappingProfile));
    }
}
