using DeLong.Domain.Common;
using DeLong.Domain.Entities;

namespace DeLong.Data.IRepository;

public interface IChangeHistoryRepository
{
    Task LogChangeAsync<T>(T oldEntity, T newEntity, long entityId, string entityName, string changeType) where T : Auditable;
    Task<ChangeHistory> GetChangeHistoryAsync(long id);
    Task<IQueryable<ChangeHistory>> GetAllChangeHistoryAsync(string entityName = null);

}
