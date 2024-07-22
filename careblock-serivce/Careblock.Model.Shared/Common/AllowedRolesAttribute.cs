using System.ComponentModel.DataAnnotations;
using Careblock.Model.Shared.Enum;

namespace careblock_service.Authorization;

public class AllowedRolesAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // Or return ValidationResult.ErrorMessage if null roles are not allowed
        }

        var allowedRoles = new[] { Role.Patient, Role.Doctor };
        var role = (Role)value;

        if (!allowedRoles.Contains(role))
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}
