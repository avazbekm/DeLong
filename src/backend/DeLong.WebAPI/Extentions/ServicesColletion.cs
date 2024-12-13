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

        services.AddScoped<IStockService, StockService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IInvoiceItemService, InvoiceItemService>();
        services.AddScoped<ICashRegisterService, CashRegisterService>();
        

        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}
