using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Validations
{
    public class Shirt_EnsureCorrectSize : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var shirt = validationContext.ObjectInstance as Shirt;
            if(shirt != null && !string.IsNullOrWhiteSpace(shirt.Gender))
            {
                if(shirt.Gender.Equals("Men", StringComparison.OrdinalIgnoreCase) && shirt.Size == 'S' )
                {
                    return new ValidationResult("For Men, the shirt size should be more than S");
                }
                else if(shirt.Gender.Equals("Women", StringComparison.OrdinalIgnoreCase) && shirt.Size != 'S')
                {
                    return new ValidationResult("For women, the shirt size should not be more than S");
                }
            }
            return ValidationResult.Success;
        }
    }
}
