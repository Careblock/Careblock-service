using System.Data;
using Careblock.Model.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Data.Repository.Common.DbContext;

public class DatabaseContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext {
    
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
    public virtual DbSet<AccountRole> AccountRoles { get; set; }
    public virtual DbSet<Appointment> Appointments { get; set; }
    public virtual DbSet<AppointmentDetail> AppointmentDetails { get; set; }
    public virtual DbSet<Department> Departments { get; set; }
    public virtual DbSet<DoctorSpecialist> DoctorSpecialists { get; set; }
    public virtual DbSet<ExaminationOption> ExaminationOptions { get; set; }
    public virtual DbSet<ExaminationPackage> ExaminationPackages { get; set; }
    public virtual DbSet<ExaminationPackageOption> ExaminationPackageOptions { get; set; }
    public virtual DbSet<ExaminationType> ExaminationTypes { get; set; }
    public virtual DbSet<ExaminationTypeOrganization> ExaminationTypeOrganizations { get; set; }
    public virtual DbSet<MedicineTypeOrganization> MedicineTypeOrganizations { get; set; }
    public virtual DbSet<Medicine> Medicines { get; set; }
    public virtual DbSet<MedicineResult> MedicineResults { get; set; }
    public virtual DbSet<MedicineType> MedicineTypes { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<NotificationType> NotificationTypes { get; set; }
    public virtual DbSet<Organization> Organizations { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<Result> Results { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Specialist> Specialists { get; set; }
    public virtual DbSet<TimeSlot> TimeSlots { get; set; }
    public virtual DbSet<ExaminationPackageSpecialist> ExaminationPackageSpecialists { get; set; }

    #endregion Tables
}