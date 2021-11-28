
using System;
using CaminhaoApi.Application.Services;
using CaminhaoApi.Infrastructure.Database.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CaminhaoApi.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc().AddWebApiConventions();

            services.AddScoped<IServiceCaminhao, ServiceCaminhao>();

            services.AddDbContext<DatabaseContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ICaminhaoContext, CaminhaoContext>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Caminhao Api",
                    Description = "Copyright © 2021 Caminhao Api",
                    TermsOfService = new Uri("https://github.com/gullitl/CaminhaoApi"),
                    Contact = new OpenApiContact
                    {
                        Name = "Plamedi Gullit Lusembo",
                        Email = "plam.gullit@outlook.com",
                        Url = new Uri("https://github.com/gullitl"),
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Caminhao Api v1"));

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
