using System.Linq.Expressions;
using Careblock.Data.Repository.Common.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Data.Repository.Common.BaseRepository;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class
{
    /// <summary>
    /// DbContext
    /// </summary>
    protected readonly IDbContext Entities;

    /// <summary>
    /// Dbset
    /// </summary>
    protected readonly DbSet<T> DbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class.
    /// </summary>
    /// <param name="context">IDbcontext</param>
    public GenericRepository(IDbContext context)
    {
        this.Entities = context;
        this.DbSet = context.Set<T>();
    }

    // /// <summary>
    // /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class.
    // /// </summary>
    // public GenericRepository()
    // {
    // }

    public virtual IQueryable<T> GetAll()
    {
        return this.DbSet;
    }

    public virtual async Task<T?> GetByIdAsync<TDataType>(TDataType id)
    {
        if (id == null || string.IsNullOrEmpty(id.ToString()))
        {
            return default(T);
        }

        switch (id)
        {
            case int:
            {
                var a = Convert.ToInt32(id);
                if (a <= 0)
                {
                    return default(T);
                }

                break;
            }
            case Guid when Guid.Parse(id.ToString() ?? string.Empty) == Guid.Empty:
                return default(T);
        }

        return await this.DbSet.FindAsync(id);
    }

    public virtual async Task<T?> DeleteByIdAsync<TDataType>(TDataType id)
    {
        var obj = await GetByIdAsync(id);
        if (obj != null)
        {
            this.Delete(obj);
        }

        return obj;
    }

    public virtual IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate, bool tracking = false)
    {
        return tracking ? this.DbSet.AsTracking().Where(predicate) : this.DbSet.AsNoTracking().Where(predicate);
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool tracking = false)
    {
        return await this.FindBy(predicate, tracking).FirstOrDefaultAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await this.DbSet.AnyAsync(predicate);
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entity)
    {
        await DbSet.AddRangeAsync(entity);
        // Entities.SaveChanges(); DO NOT SAVE CHANGE HERE
        // need to call unitOfWork.CommitAsync() to save the changes
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        var ent = (await DbSet.AddAsync(entity)).Entity;

        // Entities.SaveChanges(); DO NOT SAVE CHANGE HERE
        // need to call unitOfWork.CommitAsync() to save the changes
        return ent;
    }

    public virtual T Delete(T entity)
    {
        this.DbSet.Attach(entity);
        return this.DbSet.Remove(entity).Entity;
    }

    public virtual void DeleteRange(IEnumerable<T> entities)
    {
        this.DbSet.RemoveRange(entities);
    }

    public virtual void Edit(T entity, string propertyName = "")
    {
        // if (this.Entities == null) return;

        this.Entities.Entry(entity).State = EntityState.Modified;
        if (!string.IsNullOrWhiteSpace(propertyName))
        {
            // Todo: need confirm again about [TargetEntry!.State]
            this.Entities.Entry(entity).Reference(propertyName).TargetEntry!.State = EntityState.Modified;
        }
    }

    public T UpdateAsync(T entity, params string[] properties)
    {
        DbSet.Attach(entity);

        if (properties.Length == 0)
        {
            Entities.Entry(entity).State = EntityState.Modified;
        }
        else
        {
            for (int i = 0; i < properties.Length; i++)
            {
                Entities.Entry(entity).Property(properties[i]).IsModified = true;
            }
        }

        return entity;
    }

    public IEnumerable<T> UpdateRangeAsync(IEnumerable<T> entities, params string[] properties)
    {
        DbSet.AttachRange(entities);

        foreach (var entity in entities)
        {
            if (properties.Length == 0)
            {
                Entities.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    Entities.Entry(entity).Property(properties[i]).IsModified = true;
                }
            }
        }

        return entities;
    }
}