using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorsAPI
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
            //services.AddCors(options=> {
            //    options.AddDefaultPolicy(builder =>
            //    {
            //        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();//bütün orijinlerden gelen istekleri kabul et. header ý nolursa olsun ve method nolursa olsun anlamýnda diðer özellikler eklenir.
            //    });
            //});
            services.AddCors(options => {
                options.AddPolicy("AllowSites",
                    builder=> {
                        builder.WithOrigins("http://localhost:28358","https://www.mysite.com").AllowAnyHeader().AllowAnyMethod();//izin verilen sitelerin tüm header ve methodlarýndan gelenlere izin ver...
                });
                options.AddPolicy("AllowSites2", builder =>
                {
                    builder.WithOrigins("https://www.mysite2.com").WithHeaders(HeaderNames.ContentType, "x-custom-header");//hangi header larý kabul edicek
                });
                options.AddPolicy("AllowSites3", builder =>
                {
                    builder.WithOrigins("https://*.example.com").SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyHeader().AllowAnyMethod();//baþý önemli deðil .example.com ile bitenler
                });
                options.AddPolicy("AllowSites4", builder =>
                {
                    builder.WithOrigins("http://localhost:28358").WithMethods("POST", "GET").AllowAnyHeader();//izin verilcek metotlarýn veya controllerlarýn üstüne  [EnableCors("AllowSites4")] dikkate alýnacak policy yazýlýr. middleware kýsmýnada app.UseCors() içi boþ eklenir.

                });
            });
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CorsAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CorsAPI v1"));
            }

            app.UseRouting();
            app.UseCors();//eklenmesi þart...
            //app.UseCors("AllowSites2");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
