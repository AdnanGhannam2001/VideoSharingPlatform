using System.Security.Claims;

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            ModelState.AddModelErrors(result.Error!);
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
            ModelState.AddModelErrors(result.Error!);
            return View(dto);
        }

        await signInManager.SignInAsync(result.Value!, true);

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromServices] SignInManager<AppUser> signInManager) {
        await signInManager.SignOutAsync();

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
}