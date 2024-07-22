using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Account;
using Careblock.Model.Web.Appointment;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IAccountService
{
    Task<AccountDto> GetById(Guid id);
    Task<Guid> Register(AccountFormDto model);
    Task<AccountDto> Authenticate(string stakeId, string ipAddress);
    Task<AccountDto> RefreshToken(string token, string ipAddress);
    Task<bool> RevokeRefreshToken(string token, string ipAddress);
    Task<bool> HasAccount(string stakeId);
    Task<AccountDto> Update(Guid id, AccountRequest account);
    Task<List<DoctorDto>> FilterByOrganization(Guid organizationID);
    Task<List<PatientDto>> GetScheduledPatient(AppointmentStatus status,  Guid doctorID);
}