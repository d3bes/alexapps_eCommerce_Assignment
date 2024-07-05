using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace project.Core.filters
{
    public class ArabicCharactersAttribute : ValidationAttribute
    {
         protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success;
        }

        string input = value.ToString();

        if (ContainsArabicCharacters(input))
        {

            
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult("The field must contain Arabic characters.");
        }
    }

    private bool ContainsArabicCharacters(string input)
    {
        foreach (char c in input)
        {
            if (c >= 0x0600 && c <= 0x06FF) // Arabic block
            {
                return true;
            }
            if (c >= 0x0750 && c <= 0x077F) // Arabic Supplement block
            {
                return true;
            }
            if (c >= 0x08A0 && c <= 0x08FF) // Arabic Extended-A block
            {
                return true;
            }
            if (c >= 0xFB50 && c <= 0xFDFF) // Arabic Presentation Forms-A block
            {
                return true;
            }
            if (c >= 0xFE70 && c <= 0xFEFF) // Arabic Presentation Forms-B block
            {
                return true;
            }
        }

        return false;
    }
    
}
}