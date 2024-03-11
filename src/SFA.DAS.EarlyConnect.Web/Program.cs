using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Application.RegistrationExtensions;
using SFA.DAS.EarlyConnect.Web.AppStart;
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
    options.AddValidation();
    if (!isIntegrationTest)
    {
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    }

});

builder.Services.AddDataProtection(rootConfiguration);

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddApplicationInsightsTelemetry();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

}

app.UseHealthChecks("/ping");

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.UseEndpoints(endpointBuilder =>
{
    endpointBuilder.MapControllerRoute(
        name: "default",
        pattern: "{controller=GetAnAdviserController}/{action=Index}/");

    endpointBuilder.MapControllerRoute(
       name: "privacy",
       pattern: "privacy",
       defaults: new { controller = "Privacy", action = "Privacy" }
   );
});

app.Run();
