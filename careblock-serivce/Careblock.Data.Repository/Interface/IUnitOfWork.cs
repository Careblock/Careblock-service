using Careblock.Data.Repository.Common.BaseUnitOfWork;

namespace Careblock.Data.Repository.Interface;

public interface IUnitOfWork: IUnitOfWorkBase
{
    IAccountRepository AccountRepository { get; }
    
    IRefreshTokenRepository RefreshTokenRepository { get; }
    
    IOrganizationRepository OrganizationRepository { get; }

    IAppointmentRepository AppointmentRepository { get; }

    IMedicalServiceRepository MedicalServiceRepository { get; }

    IDiagnosticRepository DiagnosticRepository { get; }

    IExaminationResultRepository ExaminationResultRepository { get; }
}