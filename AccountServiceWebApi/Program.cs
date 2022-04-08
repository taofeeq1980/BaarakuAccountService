using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Threading.Tasks;

namespace AccountServiceWebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ConfigureLog();

            // CreateHostBuilder(args).Build().Run();
            try
            {
                var host = CreateHostBuilder(args).Build();

                Log.Information("Starting up");

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        Log.Information(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                        //var context = services.GetRequiredService<AccountServiceDbContext>();

                        //if (context.Database.IsMySql())
                        //{
                        //    await context.Database.MigrateAsync();

                        //    Log.Information("Database migration successful");
                        //}
                    }
                    catch (Exception ex)
                    {
                        Log.Information("Error Migrating database: " + ex +
                                        "\n Message: " + ex.Message + "\n Inner Exception: " + ex.InnerException);
                        throw;
                    }
                }

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.Information("Closing up");
                Log.CloseAndFlush();
            }
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .UseSerilog()
               .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        private static void ConfigureLog()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .AddJsonFile(
                                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                                    optional: true)
                                .Build();

            var loggerConfiguration = new LoggerConfiguration()
                                       .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                                       .MinimumLevel.Override("Microsoft.AspNetCore.Http.Connections", LogEventLevel.Error) // Should be relevant to SignalR logging
                                       .Enrich.FromLogContext()
                                       .Enrich.WithExceptionDetails()
                                       .Enrich.WithProperty("Company", "Baraaku")
                                       .Enrich.WithProperty("Application", "AccountServiceApi")
                                       .Enrich.WithProperty("Environment", environment)
                                       .WriteTo.Console(outputTemplate: "[{Timestamp:yyy-MM-dd HH:mm:ss.fff zzz} {Level}] {Message} ({SourceContext:l}){NewLine}{Exception}");

            loggerConfiguration = loggerConfiguration.WriteTo.Console();

            Log.Logger = loggerConfiguration.CreateLogger();
        }
    }


}
