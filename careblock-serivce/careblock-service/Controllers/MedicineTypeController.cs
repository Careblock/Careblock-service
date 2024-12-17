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
public class MedicineTypeController : BaseController
{
    private readonly IMedicineTypeService _medicineTypeService;
    
    public MedicineTypeController(IMedicineTypeService medicineTypeService)
    {
        _medicineTypeService = medicineTypeService;
    }

    #region GET

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<MedicineType>>> GetAll()
    {
        var result = await _medicineTypeService.GetAll();
        return new ApiResponse<List<MedicineType>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ApiResponse<MedicineType>> Get([FromRoute] Guid id)
    {
        var result = await _medicineTypeService.GetById(id);
        return new ApiResponse<MedicineType>(result);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<int>> Create([FromForm] MedicineTypeFormDto medicineType)
    {
        var result = await _medicineTypeService.Create(medicineType);
        return new ApiResponse<int>(result);
    }

    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ApiResponse<MedicineTypeDto>> Update([FromRoute] int id, [FromForm] MedicineTypeFormDto medicineType)
    {
        var result = await _medicineTypeService.Update(id, medicineType);
        return new ApiResponse<MedicineTypeDto>(result);
    }

    #endregion


    #region DELETE

    [AllowAnonymous]
    [HttpDelete("{id}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] int id)
    {
        var result = await _medicineTypeService.Delete(id);
        return new ApiResponse<bool>(result);
    }

    #endregion
}