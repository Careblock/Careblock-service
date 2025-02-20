﻿using Careblock.Model.Shared.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class Result
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Appointment))]
    public Guid AppointmentId { get; set; }

    // pdf url
    public string? DiagnosticUrl { get; set; }

    public string? Message { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? SignedDate { get; set; }

    public int Status { get; set; } = (int)ResultStatus.Draft;

    public string? SignHash { get; set; }

    public string? HashName { get; set; }

    public string? MulSignJson { get; set; }

    public virtual Appointment Appointment { get; set; }

    public virtual ICollection<MedicineResult> MedicineResults { get; set; }
}