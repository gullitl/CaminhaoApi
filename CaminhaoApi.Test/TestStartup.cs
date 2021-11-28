using CaminhaoApi.Application;
using CaminhaoApi.Infrastructure.Database.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http;

namespace CaminhaoApi.Test
{
    public abstract class TestStartup
    {
        protected readonly HttpClient _testClient;
        protected TestStartup()
        {
            WebApplicationFactory<Startup> appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions<DatabaseContext>));
                        services.AddDbContext<DatabaseContext>(Options =>
                        {
                            Options.UseInMemoryDatabase(databaseName: "caminhaotestdb");
                        });
                    });
                });
            IServiceScope scope = appFactory.Services.CreateScope();
            _testClient = appFactory.CreateClient();
        }
    }
}
