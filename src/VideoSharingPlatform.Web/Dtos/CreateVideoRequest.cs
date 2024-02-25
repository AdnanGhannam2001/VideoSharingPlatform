using System.ComponentModel.DataAnnotations;

using VideoSharingPlatform.Application.Validators;

namespace VideoSharingPlatform.Web.Dtos;

public class CreateVideoRequest {
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    [Display(Name = "Video")]
    [MaxFileSize(5000)]
    [Video]
    public required IFormFile VideoFile { get; set; }
    [MaxFileSize(1000)]
    [Image]
    public required IFormFile Thumbnail { get; set; }
}