using DataStore;
using Diploma.DataBase;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ProjectDiploma
{
    public class Program
    {
        private static bool NeedUpdateDatabase { get => false; }

        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            if (NeedUpdateDatabase)
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    //var context = services.GetRequiredService<BusinessUniversityContext>();
                    //context.Database.Migrate();

                    try
                    {
                        SeedData.Initialize(services).Wait();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "Error when init db");
                        throw ex;
                    }
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
