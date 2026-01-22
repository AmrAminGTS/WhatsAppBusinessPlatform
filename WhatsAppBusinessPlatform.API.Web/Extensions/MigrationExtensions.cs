using Microsoft.EntityFrameworkCore;
using WhatsAppBusinessPlatform.Infrastucture.Persistence;

namespace WhatsAppBusinessPlatform.API.Web.Extensions;
internal static class MigrationExtensions
{
    extension(IApplicationBuilder app)
    {
        public void ApplyMigrations()
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
