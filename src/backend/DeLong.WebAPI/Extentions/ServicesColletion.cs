using DeLong.Application.Interfaces;
using DeLong.Application.Mappers;
using DeLong.Infrastructure.Repositories;

namespace DeLong.WebAPI.Extentions;

public static class ServicesColletion
{
    public static void AddServices(this IServiceCollection services)
    {

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddAutoMapper(typeof(MappingProfile));
    }
}
