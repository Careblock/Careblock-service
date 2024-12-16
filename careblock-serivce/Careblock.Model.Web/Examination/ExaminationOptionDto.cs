﻿namespace Careblock.Model.Web.Examination;

public class ExaminationOptionDto
{
    public Guid? Id { get; set; }

    public Guid SpecialistId { get; set; }

    public string? SpecialistName { get; set; }

    // Blood tests, Urine tests, X-ray, Gastroscopy, Colonoscopy...
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public double? Price { get; set; }

    // by minutes
    public int? TimeEstimation { get; set; }

    // Lưu cấu trúc field input trên FE dưới dạng Json
    public string? ExaminationForm { get; set; }
}