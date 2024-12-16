using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Careblock.Data.Repository.Common.DbContext;

public interface IDbContext : IDisposable
{
    /// <summary>
    /// Dataset interface
    /// </summary>
    /// <typeparam name="T">Data Type</typeparam>
    /// <returns>Data Set</returns>
    DbSet<T> Set<T>()
        where T : class;

    /// <summary>
    /// Save changes
    /// </summary>
    /// <returns>Number records is effected</returns>
    int SaveChanges();

    /// <summary>
    /// Get entity entry
    /// </summary>
    /// <param name="o">Object</param>
    /// <returns>Entity Entry Instance </returns>
    EntityEntry Entry(object o);

    /// <summary>
    /// Mark object and property Name as modified
    /// </summary>
    /// <param name="o">Object</param>
    /// <param name="propertyName">Property name</param>
    void MarkAsModified(object o, string propertyName);

    Task<T> ExecuteStoredProcedure<T>(string storedProcedure, params SqlParameter[] parameters);
}
