using AutoMapper;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Shared.Authorization;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Account;
using Careblock.Model.Web.Organization;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Careblock.Service.Helper.JwtUtils;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Appointment;

namespace Careblock.Service.BusinessLogic;

public class AccountService : EntityService<Model.Database.Account>, IAccountService
{
    private readonly IJwtUtils _jwtUtils;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public AccountService(IJwtUtils jwtUtils,
                        IMapper mapper,
                        IOptions<AppSettings> appSettings,
                        IStorageService storageService,
                        IUnitOfWork unitOfWork)
                        : base(unitOfWork, unitOfWork.AccountRepository)
    {
        _jwtUtils = jwtUtils;
        _mapper = mapper;
        _appSettings = appSettings.Value;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AccountDto> GetById(Guid id)
    {
        var account = await _unitOfWork.AccountRepository.GetByIdAsync(id);
        if (account == null) return new AccountDto();

        return new AccountDto()
        {
            Id = account.Id,
            Avatar = account.Avatar,
            BloodType = (byte)account.BloodType,
            DateOfBirth = account.DateOfBirth,
            Firstname = account.Firstname,
            Gender = (byte)account.Gender,
            Email = account.Email,
            IdentityId = account.IdentityId,
            IsDeleted = account.IsDeleted,
            Lastname = account.Lastname,
            Phone = account.Phone,
            Role = (byte)account.Role,
            StakeId = account.StakeId,
        };
    }

    public async Task<Guid> Register(AccountFormDto model)
    {
        try
        {
            var account = await _unitOfWork.AccountRepository.FirstOrDefaultAsync(x => x.StakeId == model.StakeId);
            if (account != null)
            {
                throw new AppException("Account has been existed!");
            }

            var newAccount = _mapper.Map<Model.Database.Account>(model);
            newAccount.Id = Guid.NewGuid();
            newAccount.Avatar = await _storageService.UploadImage(model.Avatar);
            await CreateAsync(newAccount);
            return newAccount.Id;
        }
        catch (Exception e)
        {
            return Guid.Empty;
        }
    }

    public async Task<AccountDto> Authenticate(string stakeId, string ipAddress)
    {
        var account = await _unitOfWork.AccountRepository.FirstOrDefaultAsync(x => x.StakeId == stakeId);

        if (account == null)
            throw new AppException("Account not found!");

        // authentication successful so generate jwt and refresh tokens
        var jwtToken = _jwtUtils.GenerateJwtToken(account);
        var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
        refreshToken.Id = Guid.NewGuid();
        refreshToken.AccountId = account.Id;

        // remove old refresh tokens
        await RemoveOldRefreshTokens(account.Id);

        await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
        await _unitOfWork.CommitAsync();

        var organization = await _unitOfWork.OrganizationRepository.GetByIdAsync(account.OrganizationId);

        return new AccountDto()
        {
            Id = account.Id,
            StakeId = account.StakeId,
            Firstname = account.Firstname,
            Lastname = account.Lastname,
            DateOfBirth = account.DateOfBirth,
            Gender = (byte)account.Gender,
            Email = account.Email,
            IdentityId = account.IdentityId,
            BloodType = (byte)account.BloodType,
            Phone = account.Phone,
            Role = (byte)account.Role,
            IsDeleted = account.IsDeleted,
            Avatar = account.Avatar,
            Organization = organization != null ? new OrganizationDto()
            {
                Id = organization.Id,
                Code = organization.Code,
                Name = organization.Name,
                Location = organization.Location,
                Avatar = organization.Avatar,
                Description = organization.Description,
                CreatedDate = organization.CreatedDate
            } : null,
            JwtToken = jwtToken,
            RefreshToken = refreshToken.Token
        };
    }

    public async Task<AccountDto> RefreshToken(string token, string ipAddress)
    {
        // var account = await getAccountByRefreshToken(token);
        var refreshToken = await _unitOfWork.RefreshTokenRepository.GetToken(token);
        if (refreshToken == null || !refreshToken.IsActive)
            throw new AppException("Invalid token");

        var account = await _unitOfWork.AccountRepository.GetByIdAsync(refreshToken.AccountId);
        if (account == null)
            throw new AppException("Account not found");

        var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
        newRefreshToken.Id = new Guid();
        newRefreshToken.AccountId = account.Id;

        await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);
        await _unitOfWork.CommitAsync();

        var jwtToken = _jwtUtils.GenerateJwtToken(account);
        var organization = await _unitOfWork.OrganizationRepository.GetByIdAsync(account.OrganizationId);
        return new AccountDto()
        {
            Id = account.Id,
            StakeId = account.StakeId,
            Firstname = account.Firstname,
            Lastname = account.Lastname,
            DateOfBirth = account.DateOfBirth,
            Gender = (byte)account.Gender,
            IdentityId = account.IdentityId,
            Email = account.Email,
            BloodType = (byte)account.BloodType,
            Phone = account.Phone,
            Role = (byte)account.Role,
            IsDeleted = account.IsDeleted,
            Organization = organization != null ? new OrganizationDto()
            {
                Id = organization.Id,
                Code = organization.Code,
                Name = organization.Name,
                Location = organization.Location,
                Avatar = organization.Avatar,
                Description = organization.Description,
                CreatedDate = organization.CreatedDate
            } : null,
            JwtToken = jwtToken,
            RefreshToken = refreshToken.Token
        };
    }

    public async Task<bool> RevokeRefreshToken(string token, string ipAddress)
    {
        var refreshToken = await _unitOfWork.RefreshTokenRepository.GetToken(token);
        if (refreshToken == null)
            throw new AppException("Token not found");
        var a = refreshToken.IsActive;
        if (!refreshToken.IsActive)
            throw new AppException("Invalid token");

        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.ReasonRevoked = "Revoked without replacement";

        _unitOfWork.RefreshTokenRepository.Edit(refreshToken);
        return await _unitOfWork.CommitAsync() > 0;
    }

    public async Task<bool> HasAccount(string stakeId)
    {
        return await _unitOfWork.AccountRepository.AnyAsync(x => x.StakeId == stakeId);
    }

    public async Task<AccountDto> Update(Guid id, AccountRequest account)
    {
        var acc = await _unitOfWork.AccountRepository.GetByIdAsync(id);
        if (acc == null)
            throw new AppException("Account not found");

        acc.Firstname = account.Firstname;
        acc.Lastname = account.Lastname;
        acc.Email = account.Email;
        acc.DateOfBirth = account.DateOfBirth;
        acc.Gender = (Gender)account.Gender;
        acc.IdentityId = account.IdentityId;
        acc.BloodType = (BloodType)account.BloodType;
        acc.Phone = account.Phone;
        if(account.Avatar != null) acc.Avatar = await _storageService.UploadImage(account.Avatar);
        await UpdateAsync(acc);

        return new AccountDto
        {
            Avatar = acc.Avatar,
            BloodType = (byte)acc.BloodType,
            DateOfBirth = acc.DateOfBirth,
            Email = acc.Email,
            Firstname = acc.Firstname,
            Gender = (byte)acc.Gender,
            Id = acc.Id,
            IdentityId = acc.IdentityId,
            IsDeleted = acc.IsDeleted,
            Lastname = acc.Lastname,
            Phone = acc.Phone,
            Role = (byte)acc.Role,
            StakeId = acc.StakeId,
        };
    }

    public async Task<List<DoctorDto>> FilterByOrganization(Guid organizationID)
    {
        Expression<Func<Model.Database.Account, bool>> predicate = p => (p.IsDeleted == false && p.OrganizationId != Guid.Empty && Guid.Equals(organizationID, p.OrganizationId));

        var result = _unitOfWork.AccountRepository.FindBy(predicate).AsEnumerable().GroupJoin(
            _unitOfWork.AppointmentRepository.GetAll().Where(p => p.StartTime > DateTime.Now && (p.Status == AppointmentStatus.Active || p.Status == AppointmentStatus.PostPoned)).Select(x => new ExistedAppointmentDto
            {
                Id = x.Id,
                Status = x.Status,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                DoctorId = x.DoctorId,
            }),
            account => account.Id,
            appointment => appointment.DoctorId,
            (account, appointments) => new DoctorDto
            {
                Id = account.Id,
                OrganizationId = account.OrganizationId ?? Guid.Empty,
                Avatar = account.Avatar,
                DateOfBirth = account.DateOfBirth,
                Firstname = account.Firstname,
                Gender = account.Gender,
                Lastname = account.Lastname,
                Phone = account.Phone,
                Seniority = account.Seniority,
                Appointments = appointments.ToList()
            }).ToList();
        return result;
    }

    public async Task<List<PatientDto>>? GetScheduledPatient(AppointmentStatus status, Guid doctorID)
    {
        Expression<Func<Model.Database.Account, bool>> predicate = p => (p.IsDeleted == false);

        var result = _unitOfWork.AccountRepository.FindBy(predicate).AsEnumerable().Join(
            _unitOfWork.AppointmentRepository.GetAll().Where(p => (p.Status == status) && (p.DoctorId == doctorID)),
            account => account.Id,
            appointment => appointment.PatientId,
            (account, appointments) => new PatientDto
            {
                Id = account.Id,
                Avatar = account.Avatar,
                DateOfBirth = account.DateOfBirth,
                Firstname = account.Firstname,
                Gender = account.Gender,
                Lastname = account.Lastname,
                Phone = account.Phone,
                BloodType = account.BloodType,
                Role = (byte)account.Role,
                CreatedDate = appointments.CreatedDate,
                AppointmentId = appointments.Id
            }).OrderBy(x => x.CreatedDate).ToList();

        return result;
    }

    #region private Functions
    private async Task RemoveOldRefreshTokens(Guid accountId)
    {
        var refreshTokens = await _unitOfWork.RefreshTokenRepository.GetRefreshTokens(accountId);
        var oldRefreshTokens = refreshTokens.Where(x =>
            x.Expires.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow).ToList();

        if (oldRefreshTokens.Count > 0)
        {
            _unitOfWork.RefreshTokenRepository.DeleteRange(oldRefreshTokens);
            await _unitOfWork.CommitAsync();
        }
    }

    #endregion
}