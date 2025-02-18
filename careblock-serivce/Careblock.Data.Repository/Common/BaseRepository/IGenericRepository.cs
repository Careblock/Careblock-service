using System.Linq.Expressions;

namespace Careblock.Data.Repository.Common.BaseRepository;

public interface IGenericRepository<T>
{
    IQueryable<T> GetAll();

    Task<T?> GetByIdAsync<TDataType>(TDataType id);

    Task<T?> DeleteByIdAsync<TDataType>(TDataType id);

    IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, bool tracking = false);

    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool tracking = false);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    Task AddRangeAsync(IEnumerable<T> entity);

    Task<T> AddAsync(T entity);

    T Delete(T entity);

    void DeleteRange(IEnumerable<T> entities);

    void Edit(T entity, string propertyName = "");

    T UpdateAsync(T entity, params string[] properties);

    IEnumerable<T> UpdateRangeAsync(IEnumerable<T> entities, params string[] properties);
}