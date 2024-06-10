﻿using Application.Dtos.Game;
using Application.Services;
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
    [Route("/create-game")]
    public async Task<ActionResult> CreateGameAsync(GameDto game)
    {
        await _svc.AddGame(game);
        return Ok();
    }   
    
    [HttpGet]
    [Route("/get-games")]
    public async Task<ActionResult> GetGamesAsync()
    {
        var result = await _svc.GetGame();
        return Ok(result);
    }
}
