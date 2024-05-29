using Application.Dtos.Prevention;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ASP.NET_Core_Template.Controllers;

[ApiController]
public class PreventionController : ControllerBase
{
    private readonly IPreventionService svc;
    private readonly ISessionService _session;
    public PreventionController(IPreventionService svc, ISessionService session)
    {
        this.svc = svc;
        _session = session;
    }

    [HttpPost("add-prevention")]
    public async Task<ActionResult> AddPrevention(PreventionDto dto)
    {
        await svc.AddPrevention(dto);

        return Ok();
    } 
    
    [HttpGet("get-prevention")]
    public async Task<ActionResult> GetPrevention()
    {
        var preventions = await svc.GetPrevention();
        _session.Set("preventions", JsonSerializer.Serialize(preventions));

        return Ok(preventions);
    }
}
