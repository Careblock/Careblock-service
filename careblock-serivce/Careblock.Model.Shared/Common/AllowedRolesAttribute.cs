using Careblock.Model.Shared.Common;
using System.ComponentModel.DataAnnotations;

namespace careblock_service.Authorization;

public class AllowedRolesAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // Or return ValidationResult.ErrorMessage if null roles are not allowed
        }

        var allowedRoles = new[] { Constants.PATIENT, Constants.DOCTOR };
        var role = value;

        if (!allowedRoles.Contains(role))
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}
