using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoSharingPlatform.Application.Features.Commands.CreateVideo;
using VideoSharingPlatform.Application.Features.Queries.GetComments;
using VideoSharingPlatform.Application.Features.Queries.GetCommentsCount;
using VideoSharingPlatform.Application.Features.Queries.GetVideoById;
using VideoSharingPlatform.Application.Features.Queries.GetVideos;
using VideoSharingPlatform.Application.Features.Queries.GetVideosCount;
using VideoSharingPlatform.Core.Interfaces;
using VideoSharingPlatform.Web.Dtos;

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

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 2) {
        var videos = await _mediator.Send(new GetVideosQuery(pageNumber, pageSize));
        var count = await _mediator.Send(new GetVideosCountQuery());

        return View(new VideosResponse(videos.Value!, pageNumber, pageSize, count.Value));
    }

    [HttpGet("watch/{id}")]
    public async Task<IActionResult> Watch(string id) {
        var result = await _mediator.Send(new GetVideoByIdQuery(id));
        var commentsCount = await _mediator.Send(new GetCommentsCountQuery(id));

        if (!result.IsSuccess || !commentsCount.IsSuccess) {
            return NotFound();
        }

        return View(new VideoResponse(result.Value!, commentsCount.Value));
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
            dto.Description,
            Path.GetExtension(dto.VideoFile.FileName),
            Path.GetExtension(dto.Thumbnail.FileName)));

        if (!result.IsSuccess) {
            return View(dto);
        }

        await _uploadService.UploadVideoAsync(dto.VideoFile, result.Value!.Id);
        await _uploadService.UploadImageAsync(dto.Thumbnail, result.Value!.Id);

        return RedirectToAction(nameof(HomeController.Index), "home");
    }
    
    [HttpGet("comments/{id}")]
    public async Task<IActionResult> Comments(string id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 2) {
        var comments = await _mediator.Send(new GetCommentsQuery(id));
        var count = await _mediator.Send(new GetCommentsCountQuery(id));

        return View(new CommentsResponse(id, comments.Value!, pageNumber, pageSize, count.Value));
    }
}