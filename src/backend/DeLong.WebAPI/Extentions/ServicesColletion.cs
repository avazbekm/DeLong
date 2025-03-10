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
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDebtService, DebtService>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<IPriceServer, PriceService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<ISaleItemService, SaleItemService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IKursDollarService, KursDollarService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IDebtPaymentService, DebtPaymentService>();
        services.AddScoped<ICashRegisterService, CashRegisterService>();
        services.AddScoped<IReturnProductService, ReturnProductService>();
        services.AddScoped<ITransactionItemService, TransactionItemService>();


        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}
