using System.Text.Json;
using DeLong.Domain.Common;
using DeLong.Domain.Entities;
using DeLong.Data.IRepository;
using Microsoft.EntityFrameworkCore;
using DeLong.Infrastructure.Contexts;

namespace DeLong.Data.Repositories;

public class ChangeHistoryRepository : IChangeHistoryRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<ChangeHistory> _dbSet;

    public ChangeHistoryRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<ChangeHistory>();
    }

    public async Task LogChangeAsync<T>(T oldEntity, T newEntity, long entityId, string entityName, string changeType) where T : Auditable
    {
        var changeHistory = new ChangeHistory
        {
            EntityId = entityId,
            EntityName = entityName,
            ChangeType = changeType,
            OldValues = oldEntity != null ? JsonSerializer.Serialize(oldEntity) : null,
            NewValues = newEntity != null ? JsonSerializer.Serialize(newEntity) : null,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = oldEntity?.CreatedBy // Agar CreatedBy dinamik olinmasa, boshqa mexanizm qo‘shiladi
        };

        await _dbSet.AddAsync(changeHistory);
        await _context.SaveChangesAsync();
    }

    public async Task<ChangeHistory> GetChangeHistoryAsync(long id)
    {
        return await _dbSet.FirstOrDefaultAsync(ch => ch.Id == id);
    }

    public async Task<IQueryable<ChangeHistory>> GetAllChangeHistoryAsync(string entityName = null)
    {
        IQueryable<ChangeHistory> query = _dbSet.AsQueryable();
        if (!string.IsNullOrEmpty(entityName))
        {
            query = query.Where(ch => ch.EntityName == entityName);
        }
        return query.AsNoTracking();
    }
}