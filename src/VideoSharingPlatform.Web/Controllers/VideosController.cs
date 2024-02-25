using System.Security.Claims;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using VideoSharingPlatform.Application.Features.Commands.CreateVideo;
using VideoSharingPlatform.Core.Interfaces;

using VideoSharingPlatform.Web.Dtos;
using VideoSharingPlatform.Web.Services;

namespace VideoSharingPlatform.Web.Controllers;

[Route("[controller]")]
public class VideosController : Controller {
    private readonly IMediator _mediator;
    private readonly IUploadService<IFormFile> _uploadService;

    public VideosController(IMediator mediator, IUploadService<IFormFile> uploadService)
    {
        _mediator = mediator;
        _uploadService = uploadService;
    }

    [HttpGet("create")]
    [Authorize]
    public IActionResult Create() {
        return View();
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> Create([Bind] CreateVideoRequest dto) {
        if (!ModelState.IsValid) {
            return View(dto);
        }

        var result = await _mediator.Send(new CreateVideoCommand(
            HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
            dto.Title,
            dto.Description));

        if (!result.IsSuccess) {
            return View(dto);
        }

        await _uploadService.UploadVideoAsync(dto.VideoFile, result.Value!.Id);
        await _uploadService.UploadImageAsync(dto.Thumbnail, result.Value!.Id);

        return RedirectToAction(nameof(HomeController.Index), "home");
    }
}