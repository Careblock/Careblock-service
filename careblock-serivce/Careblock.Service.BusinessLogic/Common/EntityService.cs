using Careblock.Data.Repository.Common.BaseRepository;
using Careblock.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Service.BusinessLogic.Common;

public class EntityService<T> : IEntityService<T>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<T> _repository;
    
    protected EntityService(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
    {
        this._unitOfWork = unitOfWork;
        this._repository = repository;
    }

    public virtual async Task<IEnumerable<T>?> GetAllAsync()
    {
        return await this._repository.GetAll().ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync<TDataType>(TDataType id)
    {
        return await this._repository.GetByIdAsync(id);
    }

    public virtual async Task<T?> CreateAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var result = await this._repository.AddAsync(entity);
        return await this._unitOfWork.CommitAsync() > 0 ? result : default(T);
    }

    public virtual async Task<bool> UpdateAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        this._repository.Edit(entity);
        return (await this._unitOfWork.CommitAsync()) > 0;
    }

    public async Task<bool> DeleteById<TDataType>(TDataType id)
    {
        var obj = await this._repository.GetByIdAsync(id);
        if (obj == null)
        {
            throw new Exception("Couldn't find the object");
        }

        return await this.DeleteAsync(obj);
    }

    public virtual async Task<bool> DeleteAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        if (this._repository.Delete(entity) != null)
        {
            return await this._unitOfWork.CommitAsync() > 0;
        }

        return false;
    }
}