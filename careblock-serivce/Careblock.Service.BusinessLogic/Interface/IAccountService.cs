using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Account;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IAccountService
{
    Task<AccountDto> GetById(Guid id);
    Task<List<DoctorDto>> GetAllDoctor();
    Task<List<DoctorDto>> GetDoctorsOrg(Place place, Guid doctorID);
    Task<List<PatientDto>> GetScheduledPatient(AppointmentStatus status, Guid doctorID);
    Task<DataDefaultDto> GetDataDefault(Guid appointmentId);
    Task<AccountDto> Update(Guid id, AccountRequest account);
    Task<bool> RemoveDoctorFromOrg(Guid doctorID);
    Task<bool> GrantPermission(Guid userID, List<string> permissions);
    Task<Guid> Register(AccountFormDto model);
    Task<bool> HasAccount(string stakeId); 
    Task<bool> ChooseDepartment(Guid userId, ChooseDepartmentRequest request); 
    Task<AccountDto> Authenticate(AuthenticationRequest request, string ipAddress);
    Task<AccountDto> RefreshToken(string token, string ipAddress);
    Task<bool> RevokeRefreshToken(string token, string ipAddress);
}