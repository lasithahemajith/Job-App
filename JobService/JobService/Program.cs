using JobService.Data;
using Microsoft.EntityFrameworkCore;
using JobService.Services;
using JobService.Services.Interfaces;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ✅ Database (EF Core + Retry)
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure() // 🔥 important for Docker
            ));

        // ✅ Controllers
        builder.Services.AddControllers();

        // ✅ Service Layer
        builder.Services.AddScoped<IJobService, JobService.Services.JobServiceImpl>();

        // ✅ Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // 🔥 CREATE DB + APPLY MIGRATIONS (IMPORTANT)
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var retries = 5;
            while (retries > 0)
            {
                try
                {
                    db.Database.Migrate();
                    break;
                }
                catch
                {
                    retries--;
                    Console.WriteLine("Waiting for DB...");
                    Thread.Sleep(5000);
                }
            }
        }

        // ✅ Swagger UI
        app.UseSwagger();
        app.UseSwaggerUI();

        // ❗ OPTIONAL: disable HTTPS for Docker
        // app.UseHttpsRedirection();

        app.UseAuthorization();

        // ✅ Test endpoint
        app.MapGet("/", () => "Job Service Running 🚀");

        app.MapControllers();

        app.Run();
    }
}