using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Application.DTOs.Assets;
using DeLong.Application.DTOs.Stocks;
using DeLong.Application.DTOs.Invoices;
using DeLong.Application.DTOs.Products;
using DeLong.Application.DTOs.Suppliers;
using DeLong.Application.DTOs.Customers;
using DeLong.Application.DTOs.Categories;
using DeLong.Application.DTOs.Warehouses;
using DeLong.Application.DTOs.InvoiceItems;
using DeLong.Application.DTOs.Transactions;
using DeLong.Application.DTOs.CashRegisters;
using DeLong.Application.DTOs.Users;

namespace DeLong.Application.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Customer
        CreateMap<Customer, CustomerResultDto>().ReverseMap();
        CreateMap<Customer, CustomerUpdateDto>().ReverseMap();
        CreateMap<Customer, CustomerCreationDto>().ReverseMap();

        //User
        CreateMap<User, UserResultDto>();
        CreateMap<UserUpdateDto,User>();
        CreateMap<User, UserCreationDto>();

        //Asset
        CreateMap<Asset, AssetResultDto>().ReverseMap();
        CreateMap<Asset, AssetUpdateDto>().ReverseMap();
        CreateMap<Asset, AssetCreationDto>().ReverseMap();

        //CashRegister
        CreateMap<CashRegister, CashRegisterResultDto>().ReverseMap();
        CreateMap<CashRegister, CashRegisterUpdateDto>().ReverseMap();
        CreateMap<CashRegister, CashRegisterCreationDto>().ReverseMap();

        //Category
        CreateMap<Category, CategoryResultDto>().ReverseMap();
        CreateMap<Category, CategoryUpdateDto>().ReverseMap();
        CreateMap<Category, CategoryCreationDto>().ReverseMap();

        //InvoiceItem
        CreateMap<InvoiceItem, InvoiceItemResultDto>().ReverseMap();
        CreateMap<InvoiceItem, InvoiceItemUpdateDto>().ReverseMap();
        CreateMap<InvoiceItem, InvoiceItemCreationDto>().ReverseMap();

        //Invoice
        CreateMap<Invoice, InvoiceResultDto>().ReverseMap();
        CreateMap<Invoice, InvoiceUpdateDto>().ReverseMap();
        CreateMap<Invoice, InvoiceCreationDto>().ReverseMap();

        //Product
        CreateMap<Product, ProductResultDto>().ReverseMap();
        CreateMap<Product, ProductUpdateDto>().ReverseMap();
        CreateMap<Product, ProductCreationDto>().ReverseMap();

        //Stock
        CreateMap<Stock, StockResultDto>().ReverseMap();
        CreateMap<Stock, StockUpdateDto>().ReverseMap();
        CreateMap<Stock, StockCreationDto>().ReverseMap();

        //Supplier
        CreateMap<Supplier, SupplierResultDto>().ReverseMap();
        CreateMap<Supplier, SupplierUpdateDto>().ReverseMap();
        CreateMap<Supplier, SupplierCreationDto>().ReverseMap();

        //Transaction
        CreateMap<Transaction, TransactionResultDto>().ReverseMap();
        CreateMap<Transaction, TransactionUpdateDto>().ReverseMap();
        CreateMap<Transaction, TransactionCreationDto>().ReverseMap();

        //Warehouse
        CreateMap<Warehouse, WarehouseResultDto>().ReverseMap();
        CreateMap<Warehouse, WarehouseUpdatedDto>().ReverseMap();
        CreateMap<Warehouse, WarehouseCreationDto>().ReverseMap();

    }
}
