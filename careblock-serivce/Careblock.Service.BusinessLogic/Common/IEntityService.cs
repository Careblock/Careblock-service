namespace Careblock.Service.BusinessLogic.Common;

public interface IEntityService<T>
{
    /// <summary>
    /// Get all entities
    /// </summary>
    /// <returns>IEnumerable<T></returns>
    Task<IEnumerable<T>?> GetAllAsync();

    /// <summary>
    /// Get record by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T?> GetByIdAsync<TDataType>(TDataType id);

    /// <summary>
    /// Create an entity
    /// </summary>
    /// <param name="entity">T</param>
    /// <returns>Created Entity</returns>
    Task<T?> CreateAsync(T entity);

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity">T</param>
    /// <returns>true if update successfully, otherwise return false</returns>
    Task<bool> UpdateAsync(T entity);

    Task<bool> DeleteById<TDataType>(TDataType id);

    /// <summary>
    /// Delete entity
    /// </summary>
    /// <param name="entity">T</param>
    /// <returns>true if delete successfully, otherwise return false</returns>
    Task<bool> DeleteAsync(T entity);
}