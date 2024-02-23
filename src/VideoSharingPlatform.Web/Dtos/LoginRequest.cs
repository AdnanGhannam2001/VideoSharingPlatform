using System.ComponentModel.DataAnnotations;

using VideoSharingPlatform.Application.Features.Commands.ValidateUserPassword;

namespace VideoSharingPlatform.Web.Dtos;

public record LoginRequest {
    [Display(Name = "Username")]
    [Required]
    [MaxLength(25)]
    public string UserName { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public ValidateUserPasswordCommand ToValidateUserPasswordCommand() => new(UserName, Password);
}