using DeLong.Service.DTOs.CashTransfers;

namespace DeLong.Service.Interfaces;

public interface ICashTransferService
{
    ValueTask<CashTransferResultDto> AddAsync(CashTransferCreationDto dto);
    ValueTask<CashTransferResultDto> ModifyAsync(CashTransferUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<CashTransferResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<CashTransferResultDto>> RetrieveAllAsync();
    ValueTask<IEnumerable<CashTransferResultDto>> RetrieveAllByCashRegisterIdAsync(long cashRegisterId);
}