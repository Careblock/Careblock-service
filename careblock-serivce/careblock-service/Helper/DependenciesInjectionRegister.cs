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
        builder.Services.AddScoped<IResultService, ResultService>();
        builder.Services.AddScoped<IExaminationTypeService, ExaminationTypeService>();
        builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
        builder.Services.AddScoped<IExaminationPackageService, ExaminationPackageService>();
        builder.Services.AddScoped<IExaminationOptionService, ExaminationOptionService>();
        builder.Services.AddScoped<IAppointmentDetailService, AppointmentDetailService>();
        builder.Services.AddScoped<IMedicineTypeService, MedicineTypeService>();
        builder.Services.AddScoped<IMedicineService, MedicineService>();
        builder.Services.AddScoped<IDepartmentService, DepartmentService>();
        builder.Services.AddScoped<ISpecialistService, SpecialistService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<ITimeSlotService, TimeSlotService>();
    }
}