using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Web.MedicalService;
using Careblock.Model.Web.Account;
using Careblock.Service.BusinessLogic;

namespace careblock_service.Controllers;

[ApiController]
[Route("[controller]")]
public class MedicalServiceController : BaseController
{
    private readonly IMedicalServiceService _medicalServiceService;

    public MedicalServiceController(IMedicalServiceService medicalServiceService)
    {
        _medicalServiceService = medicalServiceService;
    }

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<MedicalServiceDto>>> GetAll()
    {
        var result = await _medicalServiceService.GetAll();
        return new ApiResponse<List<MedicalServiceDto>>(result, true);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ApiResponse<MedicalServiceDto?>> Get(Guid id)
    {
        bool isSucccess = true;
        var result = await _medicalServiceService.GetById(id);
        if (Guid.Equals(result.Id, Guid.Empty))
        {
            isSucccess = false;
            result = null;
        }
        return new ApiResponse<MedicalServiceDto?>(result, isSucccess);
    }

    [AllowAnonymous]
    [HttpGet("filter-by-organization/{organizationID}")]
    public async Task<ApiResponse<List<MedicalServiceDto>>> FilterByOrganization(Guid organizationID)
    {
        var result = await _medicalServiceService.FilterByOrganization(organizationID);
        return new ApiResponse<List<MedicalServiceDto>>(result, true);
    }

    [HttpPost]
    [Route("create")]
    public async Task<ApiResponse<Guid>> Create(MedicalServiceFormDto medicalService)
    {
        var result = await _medicalServiceService.Create(medicalService);
        return new ApiResponse<Guid>(result);
    }

    [HttpPost]
    [Route("update")]
    public async Task<ApiResponse<bool>> Update(MedicalServiceDto medicalService)
    {
        var result = await _medicalServiceService.Update(medicalService);
        return new ApiResponse<bool>(result);
    }

    [HttpDelete]
    public async Task<ApiResponse<bool>> Delete(Guid id)
    {
        var result = await _medicalServiceService.Delete(id);
        return new ApiResponse<bool>(result);
    }
}