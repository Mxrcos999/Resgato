using Application.Dtos.Prevention;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Template.Controllers;

[ApiController]
public class PreventionController : ControllerBase
{
    private readonly IPreventionService svc;
    public PreventionController(IPreventionService svc)
    {
        this.svc = svc;
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
        return Ok(await svc.GetPrevention());
    }
}
