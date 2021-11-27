
using System;
using Caminhao.Api.Application.Services;
using Caminhao.Api.Infrastructure.Cryptography;
using Caminhao.Api.Infrastructure.Database.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Caminhao.Api.Application
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

            services.AddCors(options =>
            {
                options.AddPolicy(Configuration["Cors:PolicyName"],
                                  builder => builder.WithOrigins(Configuration["Cors:ConfiguePolicy:Origins"])
                                                    .AllowCredentials()
                                                    .AllowAnyMethod()
                                                    .AllowAnyHeader());
            });
            services.AddDbContext<DatabaseContext>(options =>
                                                options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Keymaster API",
                    Description = "Copyright © 2020 Keymaster API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Plamedi Gullit Lusembo",
                        Email = "plam.gullit@outlook.com",
                        Url = new Uri("https://example.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under OpenApiLicense",
                        Url = new Uri("https://example.com/license"),
                    }
                });
            });

            services.AddScoped<ICryptography, DesCryptography>();
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Keymaster Api v1"));

            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(Configuration["Cors:PolicyName"]);
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
