using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Web.Examination;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Appointment;
using Careblock.Service.BusinessLogic;
using Careblock.Model.Web.Department;

namespace careblock_service.Controllers;

[AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT, Constants.MANAGER, Constants.ADMIN })]
[ApiController]
[Route("[controller]")]
public class ExaminationPackageReviewController : BaseController
{
    private readonly IExaminationPackageReviewService _examinationPackageReviewService;
    
    public ExaminationPackageReviewController(IExaminationPackageReviewService examinationPackageReviewService)
    {
        _examinationPackageReviewService = examinationPackageReviewService;
    }

    [AllowAnonymous]
    [HttpGet("get-by-examination-package")]
    public async Task<ApiResponse<List<ExaminationPackageReviewDto>>> GetByExaminationPackageId(Guid id)
    {
        var result = await _examinationPackageReviewService.GetByExaminationPackageId(id);
        return new ApiResponse<List<ExaminationPackageReviewDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("appointment/{appointmentId:guid}")]
    public async Task<ApiResponse<ExaminationPackageReviewDto?>> GetByAppointmentId([FromRoute] Guid appointmentId)
    {
        var result = await _examinationPackageReviewService.GetByAppointmentId(appointmentId);
        bool isSuccess = result?.Id != Guid.Empty;
        return new ApiResponse<ExaminationPackageReviewDto?>(isSuccess ? result : null, isSuccess);
    }


    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<bool>> Create([FromBody] ExaminationPackageReviewCreationDto input)
    {
        try
        {
            var result = await _examinationPackageReviewService.Create(input);
            return new ApiResponse<bool>(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<bool>(false);
        }
    }

    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ApiResponse<bool>> Update([FromRoute] Guid id, [FromBody] ExaminationPackageReviewCreationDto department)
    {
        try
        {
            var result = await _examinationPackageReviewService.Update(id, department);
            return new ApiResponse<bool>(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<bool>(false);
        }
       
    }

    [AllowAnonymous]
    [HttpDelete("{id:guid}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] Guid id)
    {
        try
        {
            var result = await _examinationPackageReviewService.Delete(id);
            return new ApiResponse<bool>(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ApiResponse<bool>(false);
        }
    }

}