using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.Transactions;

namespace DeLong.Service.Interfaces;

public interface ITransactionService
{
    ValueTask<TransactionResultDto> AddAsync(TransactionCreationDto dto);
    ValueTask<TransactionResultDto> ModifyAsync(TransactionUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<TransactionResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<TransactionResultDto>> RetrieveAllAsync();
}