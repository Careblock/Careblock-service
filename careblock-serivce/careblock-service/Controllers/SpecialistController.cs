using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Specialist;

namespace careblock_service.Controllers;

[AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT, Constants.MANAGER, Constants.ADMIN })]
[ApiController]
[Route("[controller]")]
public class SpecialistController : BaseController
{
    private readonly ISpecialistService _specialistService;
    
    public SpecialistController(ISpecialistService specialistService)
    {
        _specialistService = specialistService;
    }


    #region GET

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<SpecialistDto>>> GetAll()
    {
        var result = await _specialistService.GetAll();
        return new ApiResponse<List<SpecialistDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ApiResponse<Specialist>> Get([FromRoute] Guid id)
    {
        var result = await _specialistService.GetById(id);
        return new ApiResponse<Specialist>(result);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("get-by-user/{userId:guid}")]
    public async Task<ApiResponse<List<Specialist>>> GetByUserId([FromRoute] Guid userId)
    {
        var result = await _specialistService.GetByUserId(userId);
        return new ApiResponse<List<Specialist>>(result);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("assign-specialist/{userId:guid}")]
    public async Task<ApiResponse<bool>> AssignSpecialist([FromRoute] Guid userId, [FromBody] SpecialistRequest request)
    {
        var result = await _specialistService.AssignSpecialist(userId, request);
        return new ApiResponse<bool>(result, true);
    }

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<Guid>> Create([FromForm] SpecialistFormDto specialist)
    {
        var result = await _specialistService.Create(specialist);
        return new ApiResponse<Guid>(result);
    }

    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ApiResponse<SpecialistDto>> Update([FromRoute] Guid id, [FromForm] SpecialistFormDto specialist)
    {
        var result = await _specialistService.Update(id, specialist);
        return new ApiResponse<SpecialistDto>(result);
    }

    #endregion


    #region DELETE

    [AllowAnonymous]
    [HttpDelete("{id:guid}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] Guid id)
    {
        var result = await _specialistService.Delete(id);
        return new ApiResponse<bool>(result);
    }

    #endregion
}