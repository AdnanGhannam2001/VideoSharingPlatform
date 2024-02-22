using MediatR;
using Microsoft.AspNetCore.Mvc;
using VideoSharingPlatform.Application.Features.Commands.CreateUser;
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
}