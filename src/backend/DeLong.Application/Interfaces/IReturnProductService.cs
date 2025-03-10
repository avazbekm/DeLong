using DeLong.Service.DTOs;

namespace DeLong.Service.Interfaces;

public interface IReturnProductService
{
    ValueTask<ReturnProductResultDto> AddAsync(ReturnProductCreationDto dto);
    ValueTask<ReturnProductResultDto> ModifyAsync(ReturnProductUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id); // Soft delete sifatida ishlatiladi
    ValueTask<ReturnProductResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<ReturnProductResultDto>> RetrieveAllAsync();
    ValueTask<IEnumerable<ReturnProductResultDto>> RetrieveBySaleIdAsync(long saleId); // Qo‘shimcha: SaleId bo‘yicha qaytishlarni olish
    ValueTask<IEnumerable<ReturnProductResultDto>> RetrieveByProductIdAsync(long productId); // Qo‘shimcha: ProductId bo‘yicha qaytishlarni olish
}