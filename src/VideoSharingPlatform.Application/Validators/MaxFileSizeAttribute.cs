using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace VideoSharingPlatform.Application.Validators;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MaxFileSizeAttribute : ValidationAttribute {
    /// <summary>
    /// Maximum Size in KB
    /// </summary>
    private readonly int _maxSize;
    public MaxFileSizeAttribute(int maxSize) => _maxSize = maxSize * 1024;
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        var file = value as IFormFile;

        return file is not null && file.Length > _maxSize
            ? new ValidationResult($"The file size cannot exceed {(float)_maxSize / (1024 * 1024)} MB.")
            : ValidationResult.Success;
    }
}