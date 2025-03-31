using DeLong.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DeLong.WebAPI.Extentions;

public static class MigrationHelper
{
    public static void MigrateDatabase(this IApplicationBuilder app)
    {
        //using var scope = app.ApplicationServices.CreateScope();
        //using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //context.Database.Migrate();

        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Retry mexanizmi bilan migratsiyani qo‘llash
            int maxRetries = 5;
            int delaySeconds = 5;
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    dbContext.Database.Migrate();
                    Console.WriteLine("Migratsiyalar muvaffaqiyatli qo‘llanildi.");
                    break;
                }
                catch (Npgsql.PostgresException ex) when (ex.SqlState == "57P03")
                {
                    Console.WriteLine($"PostgreSQL hali tayyor emas: {ex.Message}. {delaySeconds} soniyadan keyin qayta urinish...");
                    Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                }
            }
        }
    }
}