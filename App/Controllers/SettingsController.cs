using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Template.Controllers;

[ApiController]
[Route("Settings")]
public class SettingsController : ControllerBase
{
    private readonly ISettingService settingService;

    public SettingsController(ISettingService settingService)
    {
        this.settingService = settingService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateSetting(SettingDto dto)
    {
        await settingService.CreateSetting(dto);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult> GetSetting()
    {
        var result = await settingService.GetSetting();

        return Ok(result);
    }
}
