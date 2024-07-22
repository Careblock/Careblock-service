using System.Data;
using Careblock.Model.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Data.Repository.Common.DbContext;

public class DatabaseContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext{
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public void MarkAsModified(object o, string propertyName)
    {
        this.Entry(o).Property(propertyName).IsModified = true;
    }

    public async Task<T> ExecuteStoredProcedure<T>(string storedProcedure, params SqlParameter[] parameters)
    {
        await Database.ExecuteSqlRawAsync($"EXEC {storedProcedure}", parameters);
        var outputParam = parameters.FirstOrDefault(p => p.Direction == ParameterDirection.Output);

        if (outputParam?.Value is T result)
        {
            return result;
        }

        throw new InvalidOperationException("The output value is not of the expected type.");
    }
    
    #region Tables
    
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<Organization> Organizations { get; set; }
    public virtual DbSet<Appointment> Appointments { get; set; }
    public virtual DbSet<MedicalService> MedicalServices { get; set; }
    public virtual DbSet<Diagnostic> Diagnostics { get; set; }
    public virtual DbSet<ExaminationResult> ExaminationResults { get; set; }

    // public virtual DbSet<Certificate> Certificates { get; set; }
    // public virtual DbSet<Contact> Contacts { get; set; }

    #endregion Tables
}