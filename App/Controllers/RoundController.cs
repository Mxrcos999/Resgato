using Application.Dtos.Round;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Template.Controllers;

[ApiController]
[Route("/round")]
public class RoundController : ControllerBase
{
    private readonly IRoundService _svc;
    public RoundController(IRoundService svc)
    {
        _svc = svc;
    }

    [HttpPut]
    [Route("/pass-round")]
    public async Task<ActionResult> PassRound(RoundDto dto)
    {
        return Ok(await _svc.PassRound(dto));
    }

    [HttpPost]
    [Route("/create-round")]
    public async Task<ActionResult> CreateRound(RoundDto dto)
    {
        return Ok(await _svc.AddRound(dto));
    }
}
