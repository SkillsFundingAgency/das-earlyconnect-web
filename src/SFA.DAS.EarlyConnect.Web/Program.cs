using Esfa.Recruit.Employer.Web.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Application.RegistrationExtensions;
using SFA.DAS.EarlyConnect.Web.AppStart;
using SFA.DAS.EarlyConnect.Web.Configuration;
using SFA.DAS.Provider.Shared.UI.Startup;
using SFA.DAS.Validation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

var isIntegrationTest = builder.Environment.EnvironmentName.Equals("IntegrationTest", StringComparison.CurrentCultureIgnoreCase);
var rootConfiguration = builder.Configuration.LoadConfiguration(isIntegrationTest);

builder.Services.AddOptions();
builder.Services.AddConfigurationOptions(rootConfiguration);

builder.Services.AddLogging();

builder.Services.AddServiceRegistration();

builder.Services.AddMediatRHandlers();

builder.Services.AddHealthChecks();

builder.Services.Configure<GoogleAnalyticsConfiguration>(rootConfiguration.GetSection("GoogleAnalytics"));

builder.Services.AddAuthentication(sharedOptions =>
    {
        sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.Cookie.Name = "EarlyConnect";
        options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
        options.CookieManager = new ChunkingCookieManager() { ChunkSize = 3000 };
        options.AccessDeniedPath = "/AccessDenied";
        options.LoginPath = "/AccessDenied";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    });

builder.Services.Configure<RouteOptions>(options =>
{

}).AddMvc(options =>
{
    options.Filters.AddService<GoogleAnalyticsFilter>();
    options.AddValidation();
    if (!isIntegrationTest)
    {
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    }

}).EnableCookieBanner();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseContentSecurityPolicy();

app.UseHealthChecks("/ping");

app.UseAuthentication();

app.UseRouting();

// Redirect middleware - add this section
app.Use(async (context, next) =>
{
    // Allow the root path and static files to proceed normally
    if (context.Request.Path == "/" || 
        context.Request.Path.StartsWithSegments("/css") ||
        context.Request.Path.StartsWithSegments("/js") ||
        context.Request.Path.StartsWithSegments("/images") ||
        context.Request.Path.StartsWithSegments("/lib") ||
        context.Request.Path == "/ping")
    {
        await next();
    }
    else
    {
        // Redirect everything else to the homepage
        context.Response.Redirect("/");
        return;
    }
});

app.UseAuthorization();

app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "catch-all",
        pattern: "{*url}",
        defaults: new { controller = "GetAnAdviser", action = "Index" });
});

app.Run();