using DeLong.Domain.Common;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace DeLong.Application.Interfaces;

public interface IRepository<T> where T : Auditable
{
    Task CreateAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    void Destroy(T entity);
    Task<T> GetAsync(Expression<Func<T, bool>> expression, string[] includes = null);
    IQueryable<T> GetAll(Expression<Func<T, bool>> expression = null, bool isNoTracked = true, string[] includes = null);
    Task SaveChanges();

    // Tranzaksiya metodlari (YANGI QO‘SHILDI)
    Task<IDbContextTransaction> BeginTransactionAsync();
}