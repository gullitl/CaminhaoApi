using CaminhaoApi.Application;
using CaminhaoApi.Application.Services;
using CaminhaoApi.Infrastructure.Database.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;
using System.Net.Http;

namespace CaminhaoApi.Test
{
    public abstract class StartupTest
    {
        protected readonly string _routeCaminhao = "Caminhao/";
        protected readonly HttpClient _httpClientTest;
        protected readonly IServiceCaminhao _serviceCaminhao;

        protected StartupTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
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

            _httpClientTest = appFactory.CreateClient();
            _serviceCaminhao = scope.ServiceProvider.GetServices<IServiceCaminhao>().FirstOrDefault();
        }
    }
}
