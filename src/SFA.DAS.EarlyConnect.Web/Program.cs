using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Application.RegistrationExtensions;
using SFA.DAS.EarlyConnect.Web.AppStart;

var builder = WebApplication.CreateBuilder(args);

var isIntegrationTest = builder.Environment.EnvironmentName.Equals("IntegrationTest", StringComparison.CurrentCultureIgnoreCase);
var rootConfiguration = builder.Configuration.LoadConfiguration(isIntegrationTest);

builder.Services.AddOptions();
builder.Services.AddConfigurationOptions(rootConfiguration);

builder.Services.AddLogging();
builder.Services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });

builder.Services.AddServiceRegistration();

builder.Services.AddMediatRHandlers();

builder.Services.AddHealthChecks();

builder.Services.Configure<RouteOptions>(options =>
{

}).AddMvc(options =>
{
    if (!isIntegrationTest)
    {
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    }

});


builder.Services.AddApplicationInsightsTelemetry();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

}

app.UseHealthChecks("/ping");

app.UseRouting();

app.UseStaticFiles();

app.UseEndpoints(endpointBuilder =>
{
    endpointBuilder.MapControllerRoute(
        name: "default",
        pattern: "{controller=GetAnAdviserController}/{action=Index}/");
});

app.Run();
