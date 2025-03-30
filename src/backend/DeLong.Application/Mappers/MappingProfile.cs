using AutoMapper;
using DeLong.Service.DTOs;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs.Sale;
using DeLong.Service.DTOs.Debts;
using DeLong.Service.DTOs.Prices;
using DeLong.Service.DTOs.Employee;
using DeLong.Service.DTOs.Payments;
using DeLong.Service.DTOs.SaleItems;
using DeLong.Application.DTOs.Users;
using DeLong.Service.DTOs.KursDollar;
using DeLong.Application.DTOs.Assets;
using DeLong.Service.DTOs.DebtPayments;
using DeLong.Application.DTOs.Products;
using DeLong.Application.DTOs.Suppliers;
using DeLong.Service.DTOs.CashTransfers;
using DeLong.Application.DTOs.Customers;
using DeLong.Service.DTOs.CashWarehouse;
using DeLong.Application.DTOs.Categories;
using DeLong.Service.DTOs.TransactionItems;
using DeLong.Application.DTOs.Transactions;
using DeLong.Application.DTOs.CashRegisters;
using DeLong_Desktop.ApiService.DTOs.Discounts;
using DeLong.Service.DTOs.Branchs;

namespace DeLong.Application.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Customer
        CreateMap<Customer, CustomerResultDto>().ReverseMap();
        CreateMap<Customer, CustomerUpdateDto>().ReverseMap();
        CreateMap<Customer, CustomerCreationDto>().ReverseMap();

        // User
        CreateMap<User, UserResultDto>().ReverseMap();
        CreateMap<UserUpdateDto, User>().ReverseMap();
        CreateMap<User, UserCreationDto>().ReverseMap();

        // Asset
        CreateMap<Asset, AssetResultDto>().ReverseMap();
        CreateMap<Asset, AssetUpdateDto>().ReverseMap();
        CreateMap<Asset, AssetCreationDto>().ReverseMap();

        // CashRegister
        CreateMap<CashRegister, CashRegisterResultDto>().ReverseMap();
        CreateMap<CashRegister, CashRegisterUpdateDto>().ReverseMap();
        CreateMap<CashRegister, CashRegisterCreationDto>().ReverseMap();

        // CashTransfer
        CreateMap<CashTransfer, CashTransferResultDto>().ReverseMap();
        CreateMap<CashTransfer, CashTransferUpdateDto>().ReverseMap();
        CreateMap<CashTransfer, CashTransferCreationDto>().ReverseMap();

        // CashRegister
        CreateMap<CashWarehouse, CashWarehouseResultDto>().ReverseMap();
        CreateMap<CashWarehouse, CashWarehouseUpdateDto>().ReverseMap();
        CreateMap<CashWarehouse, CashWarehouseCreationDto>().ReverseMap();

        // Category
        CreateMap<Category, CategoryResultDto>().ReverseMap();
        CreateMap<Category, CategoryUpdateDto>().ReverseMap();
        CreateMap<Category, CategoryCreationDto>().ReverseMap();

        // Product
        CreateMap<Product, ProductResultDto>().ReverseMap();
        CreateMap<Product, ProductUpdateDto>().ReverseMap();
        CreateMap<Product, ProductCreationDto>().ReverseMap();

    
        // Supplier
        CreateMap<Supplier, SupplierResultDto>().ReverseMap();
        CreateMap<Supplier, SupplierUpdateDto>().ReverseMap();
        CreateMap<Supplier, SupplierCreationDto>().ReverseMap();

        // Transaction
        CreateMap<Transaction, TransactionResultDto>().ReverseMap();
        CreateMap<Transaction, TransactionUpdateDto>().ReverseMap();
        CreateMap<Transaction, TransactionCreationDto>().ReverseMap();

        // TransactionItem
        CreateMap<TransactionItem, TransactionItemResultDto>().ReverseMap();
        CreateMap<TransactionItem, TransactionItemUpdateDto>().ReverseMap();
        CreateMap<TransactionItem, TransactionItemCreationDto>().ReverseMap();

        // Warehouse
        CreateMap<Branch, BranchResultDto>().ReverseMap();
        CreateMap<Branch, BranchUpdateDto>().ReverseMap();
        CreateMap<Branch, BranchCreationDto>().ReverseMap();

        // Price
        CreateMap<Price, PriceResultDto>().ReverseMap();
        CreateMap<Price, PriceUpdateDto>().ReverseMap();
        CreateMap<Price, PriceCreationDto>().ReverseMap();

        // DollarKurs
        CreateMap<KursDollar, KursDollarResultDto>().ReverseMap();
        CreateMap<KursDollar, KursDollarCreationDto>().ReverseMap();

        // Discount uchun mapping
        CreateMap<Discount, DiscountResultDto>().ReverseMap();
        CreateMap<DiscountCreationDto, Discount>().ReverseMap();
        CreateMap<DiscountUpdateDto, Discount>().ReverseMap();

        // Employee
        CreateMap<Employee, EmployeeResultDto>().ReverseMap();
        CreateMap<EmployeeUpdateDto, Employee>().ReverseMap();
        CreateMap<Employee, EmployeeCreationDto>().ReverseMap();

        // Sale
        CreateMap<Sale, SaleCreationDto>().ReverseMap();
        CreateMap<Sale, SaleUpdateDto>().ReverseMap();
        CreateMap<Sale, SaleResultDto>().ReverseMap();

        // Payment
        CreateMap<Payment, PaymentCreationDto>().ReverseMap();
        CreateMap<Payment, PaymentUpdateDto>().ReverseMap();
        CreateMap<Payment, PaymentResultDto>();

        // Debt
        CreateMap<Debt, DebtCreationDto>().ReverseMap();
        CreateMap<Debt, DebtUpdateDto>().ReverseMap();
        CreateMap<Debt, DebtResultDto>().ReverseMap();

        // DebtPayment
        CreateMap<DebtPayment, DebtPaymentCreationDto>().ReverseMap();
        CreateMap<DebtPayment, DebtPaymentUpdateDto>().ReverseMap();
        CreateMap<DebtPayment, DebtPaymentResultDto>().ReverseMap();

        // SaleItem mappings
        CreateMap<SaleItem, SaleItemResultDto>().ReverseMap();
        CreateMap<SaleItem, SaleItemUpdateDto>().ReverseMap();
        CreateMap<SaleItem, SaleItemCreationDto>().ReverseMap();

        // ReturnProduct mappings (YANGI QO‘SHILDI)
        CreateMap<ReturnProduct, ReturnProductCreationDto>().ReverseMap();
        CreateMap<ReturnProduct, ReturnProductUpdateDto>().ReverseMap();
        CreateMap<ReturnProduct, ReturnProductResultDto>().ReverseMap();
    }
}