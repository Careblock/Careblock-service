using Careblock.Data.Repository.Common.DbContext;

namespace Careblock.Data.Repository.Common.BaseUnitOfWork;

public abstract class UnitOfWorkBase : IUnitOfWorkBase
{
    /// <summary>
    /// true means dbContext was disposed
    /// </summary>
    protected bool Disposed;

    /// <summary>
    /// The DbContext
    /// </summary>
    protected readonly IDbContext DbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWorkBase"/> class.
    /// </summary>
    /// <param name="context">object context</param>
    protected UnitOfWorkBase(IDbContext context)
    {
        this.DbContext = context;
    }

    ~UnitOfWorkBase()
    {
        this.Dispose(false);
    }

    /// <inheritdoc />
    public async Task<int> CommitAsync()
    {
        // Save changes with the default options
        return await Task.FromResult(this.DbContext.SaveChanges());
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(true);
    }

    /// <summary>
    /// Disposes all external resources.
    /// </summary>
    /// <param name="disposing">The dispose indicator.</param>
    private void Dispose(bool disposing)
    {
        if (this.Disposed)
        {
            return;
        }

        this.DbContext.Dispose();
        this.Disposed = true;

        if (!disposing)
        {
            return;
        }
    }
}