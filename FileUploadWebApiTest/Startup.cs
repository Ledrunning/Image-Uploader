using FileUploadWebApiTest.Contracts;
using FileUploadWebApiTest.Models;
using FileUploadWebApiTest.Repository;
using FileUploadWebApiTest.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileUploadWebApiTest
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

            services.AddDbContext<FileModelContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IFileRepository, FileRepository>();
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