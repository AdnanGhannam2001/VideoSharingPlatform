using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace VideoSharingPlatform.Application.Validators;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ImageAttribute : ValidationAttribute {
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        var file = value as IFormFile;

        if (file is not null) {
            string[] allowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp"];
            var ext = Path.GetExtension(file.FileName);

            if (!allowedExtensions.Contains(ext)) {
                return new ValidationResult("Invalid image file format.");
            }
        }

        return ValidationResult.Success;
    }
}