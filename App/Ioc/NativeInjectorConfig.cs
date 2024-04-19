﻿
using App.Extensions;
using Application.Services.Identity;
using Domain.Entitites;
using Infrastructure.Context;
using Infrastructure.Repositories.BaseRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Template.Ioc
{
    public static class NativeInjectorConfig
    {
        //Injeção das dependecias
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IIdentityService, IdentityService>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowedCorsOrigins",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(configuration);
        }
    }
}
