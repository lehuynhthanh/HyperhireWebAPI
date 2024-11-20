using Microsoft.EntityFrameworkCore;

namespace HyperhireWebAPI.Domain.Repositories;

public class GenericRepository<T, TKey> : IGenericRepository<T, TKey>
    where T : class
{
    protected readonly HyperhireDbContext _dbContext;

    protected DbSet<T> DbSet => _dbContext.Set<T>();

    public IUnitOfWork UnitOfWork => _dbContext;

    public GenericRepository(HyperhireDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Delete(T entity)
    {
        DbSet.Remove(entity);
    }

    public IQueryable<T> GetAll()
    {
        return _dbContext.Set<T>();
    }

    public Task<T1?> FirstOrDefaultAsync<T1>(IQueryable<T1> query)
    {
        return query.FirstOrDefaultAsync();
    }

    public Task<T1?> SingleOrDefaultAsync<T1>(IQueryable<T1> query)
    {
        return query.SingleOrDefaultAsync();
    }

    public Task<List<T1>> ToListAsync<T1>(IQueryable<T1> query)
    {
        return query.ToListAsync();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }
    public void Update(T entity)
    {
        DbSet.Update(entity);
    }
}
