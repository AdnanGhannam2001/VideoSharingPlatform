using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoSharingPlatform.Application.Features.Commands.AddComment;
using VideoSharingPlatform.Application.Features.Commands.AddReaction;
using VideoSharingPlatform.Application.Features.Commands.CreateVideo;
using VideoSharingPlatform.Application.Features.Queries.GetComments;
using VideoSharingPlatform.Application.Features.Queries.GetCommentsCount;
using VideoSharingPlatform.Application.Features.Queries.GetReaction;
using VideoSharingPlatform.Application.Features.Queries.GetVideoById;
using VideoSharingPlatform.Application.Features.Queries.GetVideos;
using VideoSharingPlatform.Application.Features.Queries.GetVideosCount;
using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Core.Enums;
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
        Reaction? reaction = null;

        if (HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier) is not null) {
            var reactionResult = await _mediator.Send(new GetReactionQuery(id, null, HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value));

            reaction = reactionResult.Value;
        }

        if (!result.IsSuccess || !commentsCount.IsSuccess) {
            return NotFound();
        }

        return View(new VideoResponse(result.Value!, commentsCount.Value, reaction));
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
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
            dto.VideoFile,
            dto.Thumbnail));

        return !result.IsSuccess
            ? View(dto)
            : RedirectToAction(nameof(HomeController.Index), "home");
    }

    [HttpGet("{id}/comments")]
    public async Task<IActionResult> CommentsPartial(string id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 2) {
        System.Console.WriteLine(123);
        var commentsResult = await _mediator.Send(new GetCommentsQuery(id, pageNumber, pageSize));
        var countResult = await _mediator.Send(new GetCommentsCountQuery(id));

        if (!commentsResult.IsSuccess || !countResult.IsSuccess) {
            // TODO fix this
            return NotFound();
        }

        return PartialView(new CommentsResponse(id, commentsResult.Value!, pageNumber, pageSize, countResult.Value));
    }

    [HttpPost("{id}/add-comment")]
    [Authorize]
    public async Task<IActionResult> AddComment(string id, [FromForm] string content) {
        var result = await _mediator.Send(new AddCommentCommand(
            id,
            HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value,
            content));

        if (!result.IsSuccess) {
            // TODO fix this
            return BadRequest();
        }

        return RedirectToAction(nameof(Watch), new { id });
    }

    [HttpPost("{id}/react")]
    [Authorize]
    public async Task<IActionResult> ReactPartial(string id, [FromQuery] ReactionType type) {
        var result = await _mediator.Send(new AddReactionCommand(id, null,
            HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value, type));

        return PartialView(result.Value);
    }

    [HttpPost("{videoId}/{commentId}/react")]
    [Authorize]
    public async Task<IActionResult> ReactPartial(string videoId, string commentId, ReactionType type) {
        var result = await _mediator.Send(new AddReactionCommand(videoId, commentId,
            HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value, type));

        return PartialView(result.Value);
    }
}