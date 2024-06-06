using Application.Dtos.Game;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Template.Controllers;

[ApiController]
[Route("GameController")]
public class GameController : ControllerBase
{
    //private IGameService
    //public GameController()
    //{
        
    //}
    [HttpPost]
    [Route("/create-game")]
    public async Task<ActionResult> CreateGameAsync(GameDto game)
    {
        return Ok();
    }
}
