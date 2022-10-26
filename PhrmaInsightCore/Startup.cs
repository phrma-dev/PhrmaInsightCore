using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhrmaInsightCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.BrowserLink;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.Swagger;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;

namespace PhrmaInsightCore
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
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
              builder.AllowAnyMethod()
                     .AllowAnyHeader()
                     .WithOrigins("https://phrma.sharepoint.com","https://localhost:4321")
                     .AllowCredentials();
            }));


            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddJsonOptions(options => {
                options.JsonSerializerOptions.WriteIndented = true;
            });
            services.AddRazorPages();

            var connection = @"Server=tcp:phrmasqlserver.database.windows.net,1433;Initial Catalog=Phrma-dynamicsDB;Persist Security Info=False;User ID=kdudley;Password=Toby!1987;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;";
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)));
            services.AddSwaggerGen(c =>
            {
                OpenApiInfo info = new OpenApiInfo();
                info.Description = "My API";
                info.Version = "v1";
                c.SwaggerDoc("v1", info);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                // app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.Use(async (context, next) =>
            {
              context.Response.Headers.Add("X-Frame-Options", "ALLOWALL");
              await next();
            });
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseCors("MyPolicy");
            app.UseCors(
                options => options.WithOrigins("https://phrma.sharepoint.com").AllowAnyMethod()
            );
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
           
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
