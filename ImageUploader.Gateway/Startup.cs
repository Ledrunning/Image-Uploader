﻿using ImageUploader.Gateway.Contracts;
using ImageUploader.Gateway.Models;
using ImageUploader.Gateway.Repository;
using ImageUploader.Gateway.Repository.Entity;
using ImageUploader.Gateway.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageUploader.Gateway
{
    public class Startup
    {
        private const string ConnectionString = "DbConnection";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString(ConnectionString);

            services.AddDbContext<MainDatabaseContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IMainRepository<ImageEntity>, MainRepository<ImageEntity>>();
            services.AddScoped<IFileService, FileService>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseMvc();
        }
    }
}