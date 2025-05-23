using FluentValidation;
using FluentValidation.AspNetCore;
using HouseReservation.Contracts.Validators;
using HouseReservation.Core.Models;
using HouseReservation.Core.Services.Interfaces;
using HouseReservation.Infrastructure.Data;
using HouseReservation.Infrastructure.Repositories;
using HouseReservation.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options =>
    options.ResourcesPath = "Resources"
);

builder.Services
    .AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.AddRazorPages();

builder.Services.AddDbContext<HouseReservationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("HouseReservation.Infrastructure"));
});

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    //options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<HouseReservationContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole<int>>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOrManager", policy =>
        policy.RequireRole("Admin", "Manager"))
    .AddPolicy("AtLeastUser", policy =>
        policy.RequireRole("User", "Admin", "Manager"));


builder.Services.AddTransient<IEmailSender, NoOpEmailSender>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IHouseService, HouseService>();

builder.Services.AddValidatorsFromAssembly(typeof(UserCreateViewModelValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

var app = builder.Build();

using var scope = app.Services.CreateScope();
await IdentityDataSeeder.SeedAsync(scope.ServiceProvider);

var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("pl-PL"),
    new CultureInfo("de-DE"),
};
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders =
    [
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    ]
};

app.UseRequestLocalization(localizationOptions);


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages();

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
