namespace Careblock.Data.Repository.Common.BaseUnitOfWork;

public interface IUnitOfWorkBase : IDisposable
{
    /// <summary>
    /// Saves all pending changes
    /// </summary>
    /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
    Task<int> CommitAsync();
}