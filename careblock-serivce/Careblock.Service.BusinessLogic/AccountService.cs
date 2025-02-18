using Careblock.Data.Repository.Interface;
using Careblock.Model.Shared.Authorization;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Account;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Careblock.Service.Helper.JwtUtils;
using Microsoft.Extensions.Options;
using Careblock.Model.Shared.Enum;
using Careblock.Model.Database;
using Microsoft.EntityFrameworkCore;
using Careblock.Data.Repository.Common.DbContext;

namespace Careblock.Service.BusinessLogic;

public class AccountService : EntityService<Account>, IAccountService
{
    private readonly IJwtUtils _jwtUtils;
    private readonly AppSettings _appSettings;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DatabaseContext _dbContext;

    public AccountService(IJwtUtils jwtUtils,
                        IOptions<AppSettings> appSettings,
                        IStorageService storageService,
                        IUnitOfWork unitOfWork,
                        DatabaseContext dbContext)
                        : base(unitOfWork, unitOfWork.AccountRepository)
    {
        _jwtUtils = jwtUtils;
        _appSettings = appSettings.Value;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<AccountDto> GetById(Guid id)
    {
        var account = await _unitOfWork.AccountRepository.GetByIdAsync(id);
        if (account == null || account.IsDisable == true) return new AccountDto();

        return new AccountDto()
        {
            Id = account.Id,
            Avatar = account.Avatar,
            DateOfBirth = account.DateOfBirth,
            Firstname = account.Firstname,
            Gender = (byte)account.Gender,
            Email = account.Email,
            IdentityId = account.IdentityId,
            Lastname = account.Lastname ?? string.Empty,
            Phone = account.Phone,
            StakeId = account.StakeId,
            WalletAddress = account.WalletAddress,
            Roles = _dbContext.AccountRoles.Where(x => Guid.Equals(x.AccountId, account.Id)).Select(x => x.Role.Name).ToList()
        };
    }

    public async Task<List<DoctorDto>> GetAllDoctor()
    {
        return await _unitOfWork.AccountRepository.GetAll()
            .Include(a => a.AccountRoles)
            .ThenInclude(ar => ar.Role)
            .Where(a => a.AccountRoles.Any(ar => ar.Role.Id == 2) && a.IsDisable == false)
            .Select(x => new DoctorDto
            {
                Id = x.Id,
                Avatar = x.Avatar,
                CreatedDate = x.CreatedDate,
                DateOfBirth = x.DateOfBirth,
                DepartmentId = x.DepartmentId,
                Email = x.Email,
                Firstname = x.Firstname,
                Gender = x.Gender,
                IdentityId = x.IdentityId,
                IsDisable = x.IsDisable,
                Lastname = x.Lastname ?? string.Empty,
                ModifiedDate = x.ModifiedDate,
                Phone = x.Phone,
                Seniority = x.Seniority,
                StakeId = x.StakeId,
            })
            .ToListAsync();
    }

    public async Task<List<DoctorDto>> GetDoctorsOrg(Place place, Guid doctorID)
    {
        var organizationId = await _dbContext.Accounts
            .Where(acc => Guid.Equals(acc.Id, doctorID))
            .Join(_dbContext.Departments,
             acc => acc.DepartmentId,
             dep => dep.Id,
            (acc, dep) => dep.OrganizationId)
            .FirstOrDefaultAsync();

        if (Guid.Equals(organizationId, Guid.Empty)) return new List<DoctorDto>();

        if (place == Place.INCLUSIVE)
        {
            return await _dbContext.Accounts
            .Join(_dbContext.AccountRoles,
                  acc => acc.Id,
                  ar => ar.AccountId,
                  (acc, ar) => new { acc, ar })
            .Join(_dbContext.Departments,
                  combined => combined.acc.DepartmentId,
                  dep => dep.Id,
                  (combined, dep) => new { combined.acc, combined.ar, dep })
            .Join(_dbContext.Roles,
                  combined => combined.ar.RoleId,
                  role => role.Id,
                  (combined, role) => new { combined.acc, combined.ar, combined.dep, role })
            .GroupJoin(_dbContext.DoctorSpecialists,
                  combined => combined.acc.Id,
                  ds => ds.AccountId,
                  (combined, doctorSpecialists) => new { combined, doctorSpecialists })
            .SelectMany(
                  x => x.doctorSpecialists.DefaultIfEmpty(),
                  (x, doctorSpecialist) => new { x.combined, doctorSpecialist })
            .Where(x => x.combined.ar.RoleId != (int)RoleType.PATIENT && x.combined.ar.RoleId != (int)RoleType.ADMIN && Guid.Equals(x.combined.dep.OrganizationId, organizationId) && x.combined.acc.IsDisable == false)
            .GroupBy(x => new
            {
                x.combined.acc.Id,
                x.combined.acc.Avatar,
                x.combined.acc.CreatedDate,
                x.combined.acc.DateOfBirth,
                x.combined.acc.DepartmentId,
                DepartmentName = x.combined.dep.Name,
                x.combined.acc.Email,
                x.combined.acc.Firstname,
                x.combined.acc.Gender,
                x.combined.acc.IdentityId,
                x.combined.acc.IsDisable,
                x.combined.acc.Lastname,
                x.combined.acc.ModifiedDate,
                x.combined.acc.Phone,
                x.combined.acc.Seniority,
                x.combined.acc.StakeId
            })
            .Select(x => new DoctorDto
            {
                Id = x.Key.Id,
                Avatar = x.Key.Avatar,
                CreatedDate = x.Key.CreatedDate,
                DateOfBirth = x.Key.DateOfBirth,
                DepartmentId = x.Key.DepartmentId,
                DepartmentName = x.Key.DepartmentName,
                Email = x.Key.Email,
                Firstname = x.Key.Firstname,
                Gender = x.Key.Gender,
                IdentityId = x.Key.IdentityId,
                IsDisable = x.Key.IsDisable,
                Lastname = x.Key.Lastname ?? string.Empty,
                ModifiedDate = x.Key.ModifiedDate,
                Phone = x.Key.Phone,
                Seniority = x.Key.Seniority,
                StakeId = x.Key.StakeId,
                Roles = x.Select(r => r.combined.role.Name).ToList(),
                Specialist = x.Select(r => r.doctorSpecialist != null ? r.doctorSpecialist.SpecialistId : Guid.Empty).ToList(),
            })
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
        }

        return await _dbContext.Accounts
            .Join(_dbContext.AccountRoles,
                  acc => acc.Id,
                  ar => ar.AccountId,
                  (acc, ar) => new { acc, ar })
             .GroupJoin(_dbContext.Departments,
                  combined => combined.acc.DepartmentId,
                  dep => dep.Id,
                  (combined, departments) => new { combined, departments })
            .SelectMany(
                  x => x.departments.DefaultIfEmpty(),
                  (x, dep) => new { x.combined, dep })
            .Join(_dbContext.Roles,
                  combined => combined.combined.ar.RoleId,
                  role => role.Id,
                  (combined, role) => new { combined.combined.acc, combined.combined.ar, combined.dep, role })
            .GroupJoin(_dbContext.DoctorSpecialists,
                  combined => combined.acc.Id,
                  ds => ds.AccountId,
                  (combined, doctorSpecialists) => new { combined, doctorSpecialists })
            .SelectMany(
                  x => x.doctorSpecialists.DefaultIfEmpty(),
                  (x, doctorSpecialist) => new { x.combined, doctorSpecialist })
            .Where(x => x.combined.ar.RoleId != (int)RoleType.PATIENT && x.combined.ar.RoleId != (int)RoleType.ADMIN && !Guid.Equals(x.combined.dep.OrganizationId, organizationId) && x.combined.acc.IsDisable == false)
            .GroupBy(x => new
            {
                x.combined.acc.Id,
                x.combined.acc.Avatar,
                x.combined.acc.CreatedDate,
                x.combined.acc.DateOfBirth,
                x.combined.acc.DepartmentId,
                DepartmentName = x.combined.dep.Name,
                x.combined.acc.Email,
                x.combined.acc.Firstname,
                x.combined.acc.Gender,
                x.combined.acc.IdentityId,
                x.combined.acc.IsDisable,
                x.combined.acc.Lastname,
                x.combined.acc.ModifiedDate,
                x.combined.acc.Phone,
                x.combined.acc.Seniority,
                x.combined.acc.StakeId
            })
            .Select(x => new DoctorDto
            {
                Id = x.Key.Id,
                Avatar = x.Key.Avatar,
                CreatedDate = x.Key.CreatedDate,
                DateOfBirth = x.Key.DateOfBirth,
                DepartmentId = x.Key.DepartmentId,
                DepartmentName = x.Key.DepartmentName,
                Email = x.Key.Email,
                Firstname = x.Key.Firstname,
                Gender = x.Key.Gender,
                IdentityId = x.Key.IdentityId,
                IsDisable = x.Key.IsDisable,
                Lastname = x.Key.Lastname ?? string.Empty,
                ModifiedDate = x.Key.ModifiedDate,
                Phone = x.Key.Phone,
                Seniority = x.Key.Seniority,
                StakeId = x.Key.StakeId,
                Roles = x.Select(r => r.combined.role.Name).ToList(),
                Specialist = x.Select(r => r.doctorSpecialist != null ? r.doctorSpecialist.SpecialistId : Guid.Empty).ToList(),
            })
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
    }

    public async Task<List<PatientDto>> GetScheduledPatient(AppointmentStatus status, Guid doctorID)
    {
        try
        {
            var organizationId = await _dbContext.Accounts
            .Where(acc => Guid.Equals(acc.Id, doctorID))
            .Join(_dbContext.Departments,
             acc => acc.DepartmentId,
             dep => dep.Id,
            (acc, dep) => dep.OrganizationId)
            .FirstOrDefaultAsync();

            var result = await _dbContext.Appointments
            .Include(a => a.Patient)
            .Include(a => a.ExaminationPackage)
            .Where(a => a.Patient.IsDisable == false
                && a.Status == status
                && a.DoctorId != null && Guid.Equals(a.DoctorId, doctorID)
                && Guid.Equals(a.OrganizationId, organizationId)
                && a.StartDateExpectation > DateTime.Now)
            .OrderBy(a => a.StartDateExpectation)
            .Select(a => new PatientDto
            {
                Id = a.Patient.Id,
                Name = a.Name ?? string.Empty,
                Reason = a.Reason,
                ExaminationPackageName = a.ExaminationPackage.Name,
                Address = a.Address ?? string.Empty,
                Avatar = a.Patient.Avatar,
                DateOfBirth = a.Patient.DateOfBirth,
                Email = a.Email,
                Gender = a.Gender,
                Phone = a.Phone,
                StartDateExpectation = a.StartDateExpectation,
                AppointmentId = a.Id,
                WalletAddress = a.Patient.WalletAddress
            })
            .ToListAsync();

            return result;
        }
        catch (Exception)
        {
            return new List<PatientDto>();
        }
    }

    public async Task<DataDefaultDto> GetDataDefault(Guid appointmentId)
    {
        try
        {
            Appointment appointment = _dbContext.Appointments.Where(app => Guid.Equals(app.Id, appointmentId)).FirstAsync().Result;

            Guid departmentId = _dbContext.Accounts.Where(acc => Guid.Equals(acc.Id, appointment.DoctorId)).FirstAsync().Result.DepartmentId ?? Guid.Empty;
            Guid organizationId = _dbContext.Departments.Where(dep => Guid.Equals(dep.Id, departmentId)).FirstAsync().Result.OrganizationId;

            var signerWalletAddress =
                _dbContext.Accounts.Join(_dbContext.AccountRoles,
                acc => acc.Id,
                acr => acr.AccountId,
                (acc, acr) =>
                    new
                    {
                        acr.RoleId,
                        acc.DepartmentId,
                        acc.WalletAddress
                    })
                .Join(_dbContext.Departments,
                combined => combined.DepartmentId,
                dep => dep.Id,
                (combined, dep) =>
                new
                {
                    combined.RoleId,
                    dep.OrganizationId,
                    combined.WalletAddress
                })
                .Where(combined => Guid.Equals(combined.OrganizationId, organizationId) && combined.RoleId == (int)RoleType.MANAGER).FirstOrDefaultAsync().Result;

            var result = await _unitOfWork.AppointmentRepository.GetAll()
                .Include(a => a.Organization)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => Guid.Equals(a.Id, appointmentId))
                .Select(a => new DataDefaultDto
                {
                    Id = a.Id,
                    PatientId = a.Patient.Id,
                    OrganizationThumbnail = a.Organization.Thumbnail,
                    OrganizationName = a.Organization.Name,
                    OrganizationAddress = a.Organization.Address,
                    OrganizationTel = a.Organization.Tel,
                    OrganizationFax = a.Organization.Fax,
                    OrganizationUrl = a.Organization.Website,
                    FullName = a.Name ?? string.Empty,
                    Gender = a.Gender,
                    Address = a.Address,
                    DoctorId = a.Doctor != null ? a.Doctor.Id : Guid.Empty,
                    DoctorName = a.Doctor != null ? (a.Doctor.Firstname + " " + a.Doctor.Lastname) : string.Empty,
                    CreatedDate = a.CreatedDate,
                    SignerAddress = "addr_test1qzynfu0alf6jv4v3y006aqrtmrrvq0uxqf5skspzu8julgu0j5g7tqwfr9yzt3qlypl45xjvc6e86gpls4t9gx5ypmqqpge67m",
                    //SignerAddress = signerWalletAddress != null ? signerWalletAddress.WalletAddress : string.Empty,
                    WalletAddress = a.Patient.WalletAddress ?? string.Empty,
                }).FirstOrDefaultAsync();

            return result ?? new DataDefaultDto();
        }
        catch
        {
            return new DataDefaultDto();
        }
    }

    public async Task<AccountDto> Update(Guid id, AccountRequest account)
    {
        var acc = await _unitOfWork.AccountRepository.GetByIdAsync(id) ?? throw new AppException("Account not found");
        acc.Firstname = account.Firstname;
        acc.Lastname = account.Lastname;
        acc.Email = account.Email;
        acc.DateOfBirth = account.DateOfBirth;
        acc.Gender = (Gender)account.Gender;
        acc.IdentityId = account.IdentityId;
        acc.Phone = account.Phone;
        if (account.Avatar != null) acc.Avatar = await _storageService.UploadImage(account.Avatar);
        await UpdateAsync(acc);

        var roleNames = _unitOfWork.AccountRoleRepository.GetAll().Where(x => Guid.Equals(x.AccountId, id)).Select(x => x).Join(_unitOfWork.RoleRepository.GetAll(), accountRole => accountRole.RoleId, role => role.Id, (accountRole, role) => role.Name).ToList();

        return new AccountDto
        {
            Avatar = acc.Avatar,
            DateOfBirth = acc.DateOfBirth,
            Email = acc.Email,
            Firstname = acc.Firstname,
            Gender = (byte)acc.Gender,
            Id = acc.Id,
            IdentityId = acc.IdentityId,
            Lastname = acc.Lastname,
            Phone = acc.Phone,
            Roles = roleNames,
            StakeId = acc.StakeId,
        };
    }

    public async Task<bool> RemoveDoctorFromOrg(Guid doctorID)
    {
        try
        {
            var acc = await _unitOfWork.AccountRepository.GetByIdAsync(doctorID) ?? throw new AppException("Account not found");

            acc.DepartmentId = null;
            await UpdateAsync(acc);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> GrantPermission(Guid userID, List<string> permissions)
    {
        try
        {
            var accountRoles = await _dbContext.AccountRoles.Where(ar => Guid.Equals(ar.AccountId, userID)).ToListAsync();

            if (accountRoles.Any())
            {
                _dbContext.AccountRoles.RemoveRange(accountRoles);
                await _dbContext.SaveChangesAsync();
            }

            foreach (var permission in permissions)
            {
                switch (permission)
                {
                    case Constants.DOCTOR:
                        await _dbContext.AccountRoles.AddAsync(new AccountRole
                        {
                            Id = Guid.NewGuid(),
                            AccountId = userID,
                            RoleId = (int)RoleType.DOCTOR,
                        });
                        break;
                    case Constants.MANAGER:
                        await _dbContext.AccountRoles.AddAsync(new AccountRole
                        {
                            Id = Guid.NewGuid(),
                            AccountId = userID,
                            RoleId = (int)RoleType.MANAGER,
                        });
                        break;
                }
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<Guid> Register(AccountFormDto model)
    {
        try
        {
            var account = await _unitOfWork.AccountRepository.FirstOrDefaultAsync(x => x.StakeId == model.StakeId);
            if (account != null)
                throw new AppException("Account has been existed!");
            var role = await _unitOfWork.RoleRepository.FirstOrDefaultAsync(x => x.Id == (int)model.Role) ?? throw new AppException("Something went wrong!");
            Guid newId = Guid.NewGuid();

            var newAccount = new Account
            {
                Id = newId,
                Avatar = await _storageService.UploadImage(model.Avatar),
                DateOfBirth = model.DateOfBirth,
                Description = model.Description,
                Email = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Gender = model.Gender,
                IdentityId = model.IdentityId,
                IsDisable = false,
                Phone = model.Phone,
                Seniority = model.Seniority,
                StakeId = model.StakeId,
                DepartmentId = model.DepartmentId,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                AccountRoles = new List<AccountRole>
                {
                    new() {
                        Id = Guid.NewGuid(),
                        AccountId = newId,
                        RoleId = role.Id,
                    }
                }
            };

            await CreateAsync(newAccount);

            return newAccount.Id;
        }
        catch
        {
            return Guid.Empty;
        }
    }

    public async Task<bool> HasAccount(string stakeId)
    {
        return await _unitOfWork.AccountRepository.AnyAsync(x => x.StakeId == stakeId);
    }

    public async Task<bool> ChooseDepartment(Guid userId, ChooseDepartmentRequest request)
    {
        try
        {
            var noti = await _dbContext.Notifications.Where(n => Guid.Equals(n.Id, request.NotificationId)).FirstAsync() ??
                throw new AppException("Notification not found");

            _dbContext.Notifications.Remove(noti);
            _dbContext.SaveChanges();

            var acc = await _unitOfWork.AccountRepository.GetByIdAsync(userId) ?? throw new AppException("Account not found");

            acc.DepartmentId = request.DepartmentId;
            await UpdateAsync(acc);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<AccountDto> Authenticate(AuthenticationRequest request, string ipAddress)
    {
        Account? account = await _unitOfWork.AccountRepository.FirstOrDefaultAsync(x => x.StakeId == request.StakeId) ?? throw new AppException("Account not found!");

        // authentication successful so generate jwt and refresh tokens
        var jwtToken = _jwtUtils.GenerateJwtToken(account);
        var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
        refreshToken.Id = Guid.NewGuid();
        refreshToken.AccountId = account.Id;

        // remove old refresh tokens
        await RemoveOldRefreshTokens(account.Id);

        await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
        await _unitOfWork.CommitAsync();

        var organization = await _unitOfWork.OrganizationRepository.GetByIdAsync(account.DepartmentId);

        account.WalletAddress = request.WalletAdress;
        await UpdateAsync(account);

        return new AccountDto()
        {
            Id = account.Id,
            StakeId = account.StakeId,
            WalletAddress = request.WalletAdress,
            Firstname = account.Firstname,
            Lastname = account.Lastname ?? string.Empty,
            DateOfBirth = account.DateOfBirth,
            Gender = (byte)account.Gender,
            Email = account.Email,
            IdentityId = account.IdentityId,
            Phone = account.Phone,
            Avatar = account.Avatar,
            JwtToken = jwtToken,
            IsDisable = account.IsDisable,
            ModifiedDate = account.ModifiedDate,
            CreatedDate = account.CreatedDate,
            Description = account.Description,
            Seniority = account.Seniority,
            RefreshToken = refreshToken.Token,
            Roles = _dbContext.AccountRoles.Where(x => Guid.Equals(x.AccountId, account.Id)).Select(x => x.Role.Name).ToList(),
        };
    }

    public async Task<AccountDto> RefreshToken(string token, string ipAddress)
    {
        // var account = await getAccountByRefreshToken(token);
        var refreshToken = await _unitOfWork.RefreshTokenRepository.GetToken(token);
        if (refreshToken == null || !refreshToken.IsActive)
            throw new AppException("Invalid token");

        var account = await _unitOfWork.AccountRepository.GetByIdAsync(refreshToken.AccountId) ?? throw new AppException("Account not found");
        var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
        newRefreshToken.Id = new Guid();
        newRefreshToken.AccountId = account.Id;

        await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);
        await _unitOfWork.CommitAsync();

        var jwtToken = _jwtUtils.GenerateJwtToken(account);
        return new AccountDto()
        {
            Id = account.Id,
            StakeId = account.StakeId,
            Firstname = account.Firstname,
            Lastname = account.Lastname ?? string.Empty,
            DateOfBirth = account.DateOfBirth,
            Gender = (byte)account.Gender,
            IdentityId = account.IdentityId,
            Email = account.Email,
            Phone = account.Phone,
            JwtToken = jwtToken,
            RefreshToken = refreshToken.Token,
            Seniority = account.Seniority,
            Description = account.Description,
            Avatar = account.Avatar,
            CreatedDate = account.CreatedDate,
            IsDisable = account.IsDisable,
            ModifiedDate = account.ModifiedDate,
        };
    }

    public async Task<bool> RevokeRefreshToken(string token, string ipAddress)
    {
        var refreshToken = await _unitOfWork.RefreshTokenRepository.GetToken(token) ?? throw new AppException("Token not found");
        if (!refreshToken.IsActive)
            throw new AppException("Invalid token");

        refreshToken.Revoked = DateTime.Now;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.ReasonRevoked = "Revoked without replacement";

        _unitOfWork.RefreshTokenRepository.Edit(refreshToken);
        return await _unitOfWork.CommitAsync() > 0;
    }

    #region private Functions

    private async Task RemoveOldRefreshTokens(Guid accountId)
    {
        var refreshTokens = await _unitOfWork.RefreshTokenRepository.GetRefreshTokens(accountId);
        var oldRefreshTokens = refreshTokens.Where(x =>
            x.Expires.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.Now).ToList();

        if (oldRefreshTokens.Count > 0)
        {
            _unitOfWork.RefreshTokenRepository.DeleteRange(oldRefreshTokens);
            await _unitOfWork.CommitAsync();
        }
    }

    #endregion
}