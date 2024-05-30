using Application.Dtos.Round;
using Application.Services;
using Application.Services.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Template.Controllers;

[ApiController]
[Route("/round")]
public class RoundController : ControllerBase
{
    private readonly IRoundService _svc;
    private readonly IIdentityService _svcUser;
    public RoundController(IRoundService svc, IIdentityService svcUser)
    {
        _svc = svc;
        _svcUser = svcUser;
    }

    [HttpPut]
    [Route("/pass-round")]
    public async Task<ActionResult> PassRound(RoundDto model)
    {
        return Ok(await _svc.PassRound(model));
    }

    [HttpPost]
    [Route("/answer-round")]
    public async Task<ActionResult> AnswerRound(AnswerRoundDto dto)
    {
        return Ok(await _svcUser.AnswerRound(dto));
    }

    [HttpPost]
    [Route("/create-round")]
    public async Task<ActionResult> CreateRound(RoundDto dto)
    {
        return Ok(await _svc.AddRound(dto));
    }
}
