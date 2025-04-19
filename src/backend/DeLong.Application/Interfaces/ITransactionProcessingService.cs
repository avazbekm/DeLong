
using DeLong.Service.DTOs.ReceiveItems;
using DeLong.Application.DTOs.Transactions;

namespace DeLong.Service.Interfaces;

public interface ITransactionProcessingService
{
    Task<TransactionResultDto> ProcessTransactionAsync(List<ReceiveItemDto> receiveItems);
    Task<TransactionResultDto> ProcessTransactionAsync(List<ReceiveItemDto> receiveItems, Guid? requestId);
}
