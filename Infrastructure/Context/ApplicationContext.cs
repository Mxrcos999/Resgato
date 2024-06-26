﻿using Domain.Entitites;
using Infrastructure.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Context
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public readonly IHttpContextAccessor _contextAccessor;
        public ApplicationContext(DbContextOptions<ApplicationContext> options, IHttpContextAccessor contextAccessor)
            : base(options)
        {
            _contextAccessor = contextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ApplicationUserMap());     
            
            builder.ApplyConfiguration(new GameMap());

        }
        public DbSet<PreventionAction> Actions { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<SettingCat> SettingCat { get; set; }
        public DbSet<Round> Round { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<GameStudent> GameStudent { get; set; }
    }
}
