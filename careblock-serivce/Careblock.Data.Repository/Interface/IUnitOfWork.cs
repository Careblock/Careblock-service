using Careblock.Data.Repository.Common.BaseUnitOfWork;

namespace Careblock.Data.Repository.Interface;

public interface IUnitOfWork: IUnitOfWorkBase
{
    IAccountRepository AccountRepository { get; }

    IAccountRoleRepository AccountRoleRepository { get; }

    IRefreshTokenRepository RefreshTokenRepository { get; }
    
    IOrganizationRepository OrganizationRepository { get; }

    IDepartmentRepository DepartmentRepository { get; }

    IExaminationTypeRepository ExaminationTypeRepository { get; }

    IPaymentMethodRepository PaymentMethodRepository { get; }

    IMedicineTypeRepository MedicineTypeRepository { get; }

    IMedicineRepository MedicineRepository { get; }

    ISpecialistRepository SpecialistRepository { get; }

    IExaminationOptionRepository ExaminationOptionRepository { get; }

    ITimeSlotRepository TimeSlotRepository { get; }

    IExaminationPackageRepository ExaminationPackageRepository { get; }

    IIExaminationPackageOptionRepository ExaminationPackageOptionRepository { get; }

    IAppointmentRepository AppointmentRepository { get; }

    IAppointmentDetailRepository AppointmentDetailRepository { get; }

    IResultRepository ResultRepository { get; }

    IRoleRepository RoleRepository { get; }

    INotificationRepository NotificationRepository { get; }
}