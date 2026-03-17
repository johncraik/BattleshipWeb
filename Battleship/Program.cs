using Battleship;
using Battleship.Data;
using Battleship.Models.Battleship;
using Battleship.Models.Games;
using Battleship.Models.Identity;
using JC.Communication.Email.Models;
using JC.Communication.Extensions;
using JC.Core.Extensions;
using JC.Github.Extensions;
using JC.Identity.Extensions;
using JC.MySql;
using JC.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ── Core ────────────────────────────────────────────────────
builder.Services.AddCore<ApplicationDbContext>();
builder.Services.AddMySqlDatabase<ApplicationDbContext>(builder.Configuration, migrationsAssembly: "Battleship");

builder.Services.RegisterRepositoryContexts(
    typeof(Commander),
    typeof(Ship),
    typeof(ShipCell),
    typeof(Game),
    typeof(GameLobby),
    typeof(ShipPlacement),
    typeof(BoardCellAction),
    typeof(ShipHit));

// ── Identity ────────────────────────────────────────────────
builder.Services.AddIdentity<ApplicationUser, ApplicationRole, ApplicationDbContext>();

// ── Web defaults (security headers, cookies, client profiling) ──
builder.Services.AddWebDefaults(builder.Configuration);

// ── Communication ───────────────────────────────────────────
builder.Services.AddEmail<ApplicationDbContext>(builder.Configuration, options =>
{
    options.Provider = builder.Environment.IsProduction() 
        ? EmailProvider.Microsoft 
        : EmailProvider.Console;
});
builder.Services.AddNotifications<ApplicationDbContext>();
builder.Services.AddMessaging<ApplicationDbContext>();

// ── Github ──────────────────────────────────────────────────
builder.Services.AddGithub<ApplicationDbContext>(builder.Configuration);

// ── Background Jobs ─────────────────────────────────────────
builder.ConfigureJobs();

// ── Razor Pages ─────────────────────────────────────────────
builder.Services.AddRazorPages();

var app = builder.Build();

// ── Middleware ───────────────────────────────────────────────
app.UseWebDefaults();
app.UseStaticFiles();
app.UseRouting();
app.UseIdentity();
//app.UseGithubWebhooks();

app.MapRazorPages();

// ── Seed roles and admin ────────────────────────────────────
await app.Services.MigrateDatabaseAsync<ApplicationDbContext>();
await app.ConfigureAdminAndRolesAsync<ApplicationUser, ApplicationRole, ApplicationDbContext, AppRoles>();

app.Run();
