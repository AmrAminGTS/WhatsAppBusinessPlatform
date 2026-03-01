using System.Globalization;
using WhatsAppBusinessPlatform.API.Web.Extensions;
using WhatsAppBusinessPlatform.API.Web.Localization;
using WhatsAppBusinessPlatform.Application;
using WhatsAppBusinessPlatform.Infrastucture;
using WhatsAppBusinessPlatform.API.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGenWithAuth();

// Add services to the container.
builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Logging
    .ClearProviders()
    .AddConsole()
    .AddDebug();

builder.Services.AddControllers().AddDataAnnotationsLocalization(options
    => options.DataAnnotationLocalizerProvider = (type, factory)
        => factory.Create(typeof(SharedResource)));

WebApplication app = builder.Build();

// IMPORTANT: UseRequestLocalization early in pipeline 
app.UseRequestLocalization(options => {
    List<CultureInfo> supportedCultures = [new CultureInfo("en-US"), new CultureInfo("ar-EG")];

    options.SetDefaultCulture("en-US")
    .AddSupportedCultures(supportedCultures.Select(c => c.Name).ToArray())
    .AddSupportedUICultures(supportedCultures.Select(c => c.Name).ToArray());
});

app.UseStaticFiles();
app.UseSwaggerWithUi();
app.ApplyMigrations();

// 1. Error handling (early)
app.UseExceptionHandler();

// 2. Security headers / redirects
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
app.UseHttpsRedirection();

// 3. Static files (served without auth unless you specifically protect them)
app.UseStaticFiles();

// 4. Routing must run before auth/CORS when using endpoint routing
app.UseRouting();

// 5. CORS for endpoint routing: between UseRouting and UseAuthorization/MapControllers
app.UseCors("RealTimePolicy");

// 6. Authentication & Authorization (auth before endpoints)
app.UseAuthentication();
app.UseAuthorization();

// 7. Map endpoints (controllers, minimal APIs)
app.MapControllers();
app.MapServerSentEventEndpoints();
app.MapSignalRHubs();

await app.RunAsync();
