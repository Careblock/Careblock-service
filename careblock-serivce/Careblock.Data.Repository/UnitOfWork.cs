using Careblock.Data.Repository.Common.BaseUnitOfWork;
using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;

namespace Careblock.Data.Repository;

public class UnitOfWork : UnitOfWorkBase, IUnitOfWork
{
    private AccountRepository _accountRepository;
    private AccountRoleRepository _accountRoleRepository;
    private RefreshTokenRepository _refreshTokenRepository;
    private OrganizationRepository _organizationRepository;
    private DepartmentRepository _departmentRepository;
    private AppointmentRepository _appointmentRepository;
    private IAppointmentDetailRepository _appointmentDetailRepository;
    private ResultRepository _resultRepository;
    private RoleRepository _roleRepository;
    private ExaminationTypeRepository _examinationTypeRepository;
    private PaymentMethodRepository _paymentMethodRepository;
    private MedicineTypeRepository _medicineTypeRepository;
    private MedicineRepository _medicineRepository;
    private ExaminationOptionRepository _examinationOptionRepository;
    private TimeSlotRepository _timeSlotRepository;
    private ExaminationPackageRepository _examinationPackageRepository;
    private ExaminationPackageOptionRepository _examinationPackageOptionRepository;
    private SpecialistRepository _specialistRepository;
    private NotificationRepository _notificationRepository;

    public UnitOfWork(IDbContext context) : base(context)
    {
    }

    public IAccountRepository AccountRepository => _accountRepository ??= new AccountRepository(DbContext);

    public IAccountRoleRepository AccountRoleRepository => _accountRoleRepository ??= new AccountRoleRepository(DbContext);

    public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository ??= new RefreshTokenRepository(DbContext);
    
    public IOrganizationRepository OrganizationRepository => _organizationRepository ??= new OrganizationRepository(DbContext);

    public IDepartmentRepository DepartmentRepository => _departmentRepository ??= new DepartmentRepository(DbContext);

    public IAppointmentRepository AppointmentRepository => _appointmentRepository ??= new AppointmentRepository(DbContext);

    public IAppointmentDetailRepository AppointmentDetailRepository => _appointmentDetailRepository ??= new AppointmentDetailRepository(DbContext);

    public IResultRepository ResultRepository => _resultRepository ??= new ResultRepository(DbContext);

    public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(DbContext);

    public IExaminationTypeRepository ExaminationTypeRepository => _examinationTypeRepository ??= new ExaminationTypeRepository(DbContext);

    public IPaymentMethodRepository PaymentMethodRepository => _paymentMethodRepository ??= new Repository.PaymentMethodRepository(DbContext);

    public IMedicineTypeRepository MedicineTypeRepository => _medicineTypeRepository ??= new MedicineTypeRepository(DbContext);
    
    public IMedicineRepository MedicineRepository => _medicineRepository ??= new MedicineRepository(DbContext);

    public IExaminationOptionRepository ExaminationOptionRepository => _examinationOptionRepository ??= new ExaminationOptionRepository(DbContext);

    public ITimeSlotRepository TimeSlotRepository => _timeSlotRepository ??= new TimeSlotRepository(DbContext);

    public IExaminationPackageRepository ExaminationPackageRepository => _examinationPackageRepository ??= new ExaminationPackageRepository(DbContext);

    public IIExaminationPackageOptionRepository ExaminationPackageOptionRepository => _examinationPackageOptionRepository ??= new ExaminationPackageOptionRepository(DbContext);

    public ISpecialistRepository SpecialistRepository => _specialistRepository ??= new SpecialistRepository(DbContext);

    public INotificationRepository NotificationRepository => _notificationRepository ??= new NotificationRepository(DbContext);
}