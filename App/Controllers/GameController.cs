using Application.Dtos.Game;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Template.Controllers;

[ApiController]
[Route("GameController")]
public class GameController : ControllerBase
{
    private IGameService _svc;
    public GameController(IGameService svc)
    {
        _svc = svc;
    }
    [HttpPost]
    [Authorize]
    [Route("/create-game")]
    public async Task<ActionResult> CreateGameAsync(GameDto game)
    {
        await _svc.AddGame(game);
        return Ok();
    }   
    
    [HttpPatch]
    [Authorize]
    [Route("/finish-game")]
    public async Task<ActionResult> FinishGameAsync(int gameId)
    {
        await _svc.FinishGame(gameId);
        return Ok();
    }     
    
    [HttpGet]
    [Authorize]
    [Route("/get-games")]
    public async Task<ActionResult> GetGamesAsync()
    {
        var result = await _svc.GetGame();
        return Ok(result);
    }  
    
    [HttpGet]
    [Authorize]
    [Route("/get-game-information")]
    public async Task<ActionResult> GetGameByIdAsync(int id)
    {
        var result = await _svc.GetInformationGame(id);

        if(result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("/get-players")]
    public async Task<ActionResult> GetPlayers(int id)
    {
        var result = await _svc.GetPlayers(id);

        if(result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("/get-answers")]
    public async Task<ActionResult> GetAnswer(int id)
    {
        var result = await _svc.GetResultsAsync(id);

        if(result is null)
            return NotFound();

        return Ok(result);
    }
}
