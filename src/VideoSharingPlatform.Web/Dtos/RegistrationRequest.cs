using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

using VideoSharingPlatform.Application.Features.Commands.CreateUser;

namespace VideoSharingPlatform.Web.Dtos;

public record RegisterationRequest {
    [Display(Name = "Username")]
    [Required]
    [MaxLength(25)]
    public string UserName { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Display(Name = "Password Confirmation")]
    [Required]
    [Compare(nameof(Password))]    
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = "";

    public string? DbUpdateError { get; set; }

    public CreateUserCommand ToCreateUserCommand() => new(UserName, Email, Password);
}