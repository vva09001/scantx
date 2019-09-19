using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spec_Project.Services;
using Spec_Project.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Lucene.Net.Support;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Spec_Project.Models;

namespace Spec_Project
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
            services.AddHttpContextAccessor();
            services.AddCors(o => o.AddPolicy("AllowAnyOrigin", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddDbContext<DataContext>(opt => opt.UseSqlServer(Configuration["ConnectionString"]));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper();
            // configure DI for application services
            services.AddScoped<IStartService, StartService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IScanDataService, ScanDataService>();
            services.AddScoped<IUserService, UserService>();
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<Helpers.AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
           {
             x.Events = new JwtBearerEvents
             {
                 OnTokenValidated = context =>
                 {
                     var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                     // var saleService = context.HttpContext.RequestServices.GetRequiredService<IReportSales>();
                     var userId = int.Parse(context.Principal.Identity.Name);
                     var user = userService.GetById(userId);
                     if (user == null)
                     {
                         // return unauthorized if user no longer exists
                         context.Fail("Unauthorized");
                     }
                     return Task.CompletedTask;
                 }
             };
             x.RequireHttpsMetadata = false;
             x.SaveToken = true;
             x.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(key),
                 ValidateIssuer = false,
                 ValidateAudience = false
             };
         });

            services.AddDirectoryBrowser();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("AllowAnyOrigin");
            app.UseCors(options => options.AllowAnyOrigin());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "tmp")),
                RequestPath = "/mycsv"
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "tmp")),
                RequestPath = "/mycsv"
            });
        }
    }
}
