using Careblock.Data.Repository.Common.BaseUnitOfWork;
using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;

namespace Careblock.Data.Repository;

public class UnitOfWork : UnitOfWorkBase, IUnitOfWork
{
    private AccountRepository _accountRepository;
    private RefreshTokenRepository _refreshTokenRepository;
    private OrganizationRepository _organizationRepository;
    private AppointmentRepository _appointmentRepository;
    private MedicalServiceRepository _medicalServiceRepository;
    private DiagnosticRepository _diagnosticRepository;
    private ExaminationResultRepository _examinationResultRepository;

    public UnitOfWork(IDbContext context) : base(context)
    {
    }

    public IAccountRepository AccountRepository => _accountRepository ??= new AccountRepository(DbContext);
    
    public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository ??= new RefreshTokenRepository(DbContext);
    
    public IOrganizationRepository OrganizationRepository => _organizationRepository ??= new OrganizationRepository(DbContext);

    public IAppointmentRepository AppointmentRepository => _appointmentRepository ??= new AppointmentRepository(DbContext);

    public IMedicalServiceRepository MedicalServiceRepository => _medicalServiceRepository ??= new MedicalServiceRepository(DbContext);

    public IDiagnosticRepository DiagnosticRepository => _diagnosticRepository ??= new DiagnosticRepository(DbContext);

    public IExaminationResultRepository ExaminationResultRepository => _examinationResultRepository ??= new    ExaminationResultRepository(DbContext);
}