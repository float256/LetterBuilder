using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterBuilderCore.Services;
using LetterBuilderCore.Services.DAO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LetterBuilderWebApi
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
            services.AddCors();
            services.AddControllers();
            services.AddScoped<ICatalogDataAccess, CatalogDataAccess>(x => new CatalogDataAccess(Configuration.GetConnectionString("default")));
            services.AddScoped<ITextBlockDataAccess, TextBlockDataAccess>(x => new TextBlockDataAccess(Configuration.GetConnectionString("default")));
            services.AddScoped<IDirectorySystemReadFacade, DirectorySystemFacade>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(builder => builder.WithOrigins("http://localhost:52389", "https://localhost:44355",
                "https://localhost:52389", "http://localhost:44355", "http://letterbuilder.local"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
