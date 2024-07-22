using Careblock.Data.Repository;
using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Service.BusinessLogic;
using Careblock.Service.BusinessLogic.Interface;
using Careblock.Service.Helper.JwtUtils;
using Microsoft.EntityFrameworkCore;

namespace careblock_service.Helper;

public static class DependenciesInjectionRegister
{
    public static void RegisterDependencies(this WebApplicationBuilder builder)
    {
        var dbConfig = new DatabaseConfig();
        builder.Configuration.GetSection("DatabaseConfig").Bind(dbConfig);
        
        builder.Services.AddDbContext<DatabaseContext>(options => options
            .UseSqlServer(builder.Configuration["ConnectionString"], action =>
            {
                action.CommandTimeout(dbConfig.TimeoutTime);
            })
            .EnableDetailedErrors(dbConfig.DetailedError)
            .EnableSensitiveDataLogging(dbConfig.SensitiveDataLogging)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        );

        builder.Services.AddScoped<IDbContext, DatabaseContext>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IJwtUtils, JwtUtils>();
        builder.Services.AddScoped<IStorageService, StorageService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IOrganizationService, OrganizationService>();
        builder.Services.AddScoped<IAppointmentService, AppointmentService>();
        builder.Services.AddScoped<IMedicalServiceService, MedicalServiceService>();
        builder.Services.AddScoped<IDiagnosticService, DiagnosticService>();
        builder.Services.AddScoped<IExaminationResultService, ExaminationResultService>();
    }
}