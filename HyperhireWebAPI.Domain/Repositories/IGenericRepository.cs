using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperhireWebAPI.Domain.Repositories;

public interface IGenericRepository<TEntity, TKey>
        where TEntity : class
{
    IUnitOfWork UnitOfWork { get; }

    IQueryable<TEntity> GetAll();

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    void Delete(TEntity entity);
    void Update(TEntity entity);

    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query);

    Task<T?> SingleOrDefaultAsync<T>(IQueryable<T> query);

    Task<List<T>> ToListAsync<T>(IQueryable<T> query);
}