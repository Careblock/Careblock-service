﻿using Microsoft.AspNetCore.Http;

namespace Careblock.Model.Web.Appointment;

public class AppointmentDetailFormDto
{
    public Guid ExaminationOptionId { get; set; }

    public Guid AppointmentId { get; set; }

    public Guid DoctorId { get; set; }

    // json - lưu cả cục dữ liệu
    public string Diagnostic { get; set; } = string.Empty;

    public double? Price { get; set; }

    public IFormFile? FilePDF { get; set; }

    public string? ResultId { get; set; }

    public string? ResultName { get; set; }

    public string? SignHash { get; set; }

    public string? ManagerWalletAddress { get; set; }
}