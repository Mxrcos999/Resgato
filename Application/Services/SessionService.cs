﻿using Microsoft.AspNetCore.Http;

namespace Application.Services;

public interface ISessionService
{
    void Set(string key, string value);
    string Get(string key);
}

public class SessionService : ISessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Set(string key, string value)
    {
        _httpContextAccessor.HttpContext.Session.SetString(key, value);
    }

    public string Get(string key)
    {
        return _httpContextAccessor.HttpContext.Session.GetString(key);
    }
}
