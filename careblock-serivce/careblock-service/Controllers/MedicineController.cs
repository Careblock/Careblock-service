using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Medicine;

namespace careblock_service.Controllers;

[AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT, Constants.MANAGER, Constants.ADMIN })]
[ApiController]
[Route("[controller]")]
public class MedicineController : BaseController
{
    private readonly IMedicineService _medicineService;
    
    public MedicineController(IMedicineService medicineService)
    {
        _medicineService = medicineService;
    }

    #region GET

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<MedicineDto>>> GetAll()
    {
        var result = await _medicineService.GetAll();
        return new ApiResponse<List<MedicineDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("get-by-type/{medicineTypeId:int}")]
    public async Task<ApiResponse<List<MedicineResponseDto>>> GetByType([FromRoute] int medicineTypeId)
    {
        var result = await _medicineService.GetByType(medicineTypeId);
        return new ApiResponse<List<MedicineResponseDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("get-by-type-organization/{medicineTypeId:int}/{organizationId:Guid}")]
    public async Task<ApiResponse<List<MedicineResponseDto>>> GetByTypeAndOrganization([FromRoute] int medicineTypeId, [FromRoute] Guid organizationId)
    {
        var result = await _medicineService.GetByTypeAndOrganization(medicineTypeId, organizationId);
        return new ApiResponse<List<MedicineResponseDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("get-by-organization/{userId:Guid}")]
    public async Task<ApiResponse<List<MedicineResponseDto>>> GetByOrganization([FromRoute] Guid userId)
    {
        var result = await _medicineService.GetByOrganization(userId);
        return new ApiResponse<List<MedicineResponseDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ApiResponse<Medicine>> Get([FromRoute] Guid id)
    {
        var result = await _medicineService.GetById(id);
        return new ApiResponse<Medicine>(result);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("create/{userId}")]
    public async Task<ApiResponse<Guid>> Create([FromForm] MedicineFormDto medicine, [FromRoute] Guid userId)
    {
        var result = await _medicineService.Create(medicine, userId);
        return new ApiResponse<Guid>(result);
    }

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<Guid>> CreateNew([FromForm] MedicineFormDto medicine)
    {
        var result = await _medicineService.CreateNew(medicine);
        return new ApiResponse<Guid>(result);
    }

    #endregion


    #region PUT

    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ApiResponse<MedicineDto>> Update([FromRoute] Guid id, [FromForm] MedicineFormDto medicine)
    {
        var result = await _medicineService.Update(id, medicine);
        return new ApiResponse<MedicineDto>(result);
    }

    #endregion


    #region DELETE

    [AllowAnonymous]
    [HttpDelete("{id:guid}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] Guid id)
    {
        var result = await _medicineService.Delete(id);
        return new ApiResponse<bool>(result);
    }

    #endregion
}