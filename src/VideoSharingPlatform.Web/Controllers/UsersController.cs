using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VideoSharingPlatform.Application.Features.Commands.UpdateUser;
using VideoSharingPlatform.Application.Features.Queries.GetUser;
using VideoSharingPlatform.Core.Entities.AppUserAggregate;
using VideoSharingPlatform.Web.Dtos;

namespace VideoSharingPlatform.Web.Controllers;

[Route("[controller]")]
public class UsersController : Controller {
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet("register")]
    public IActionResult Register() {
        return View();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [Bind("UserName", "Email", "Password", "ConfirmPassword")] RegisterationRequest dto)
    {
        if (!ModelState.IsValid) {
            return View(dto);
        }

        var result = await _mediator.Send(dto.ToCreateUserCommand());

        if (!result.IsSuccess) {
            ModelState.AddModelErrors(result.Exceptions);
            return View(dto);
        }

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpGet("login")]
    public IActionResult Login() {
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [Bind("UserName", "Password")] LoginRequest dto,
        [FromServices] SignInManager<AppUser> signInManager)
    {
        if (!ModelState.IsValid) {
            return View(dto);
        }

        var result = await _mediator.Send(dto.ToValidateUserPasswordCommand());

        if (!result.IsSuccess) {
            ModelState.AddModelErrors(result.Exceptions);
            return View(dto);
        }

        await signInManager.SignInAsync(result.Value, true);

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromServices] SignInManager<AppUser> signInManager) {
        await signInManager.SignOutAsync();

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> Profile([FromQuery] bool updated = false) {
        var result = await _mediator.Send(
                new GetUserQuery(
                    HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value));

        ViewData["updated"] = updated;

        return result.IsSuccess
            ? View(result.Value)
            : NotFound();
    }

    [HttpPost("update")]
    [Authorize]
    public async Task<IActionResult> Update([Bind] UserDto dto) {
        var result = await _mediator.Send(
                new UpdateUserCommand(
                    HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                    dto.UserName,
                    dto.Email,
                    dto.PhoneNumber));

        return RedirectToAction(nameof(Profile), new { updated = true });
    }
}