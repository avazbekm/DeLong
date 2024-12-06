using DeLong.Application.Interfaces;
using DeLong.Application.Mappers;
using DeLong.Infrastructure.Repositories;
using DeLong.Service.Interfaces;
using DeLong.Service.Services;

namespace DeLong.WebAPI.Extentions;

public static class ServicesColletion
{
    public static void AddServices(this IServiceCollection services)
    {

        services.AddScoped<ICustomerService, CustomerService>();
        

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddAutoMapper(typeof(MappingProfile));
    }
}
