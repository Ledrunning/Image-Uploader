using ImageUploader.Gateway.Contracts;
using ImageUploader.Gateway.Models;
using ImageUploader.Gateway.Repository;
using ImageUploader.Gateway.Repository.Entity;
using ImageUploader.Gateway.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

            services.AddLogging();
            services.AddDbContext<MainDatabaseContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IMainRepository<FileEntity>, MainRepository<FileEntity>>();
            services.AddScoped<IFileService, FileService>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}