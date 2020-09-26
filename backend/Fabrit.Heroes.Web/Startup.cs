using Fabrit.Heroes.Business.Services;
using Fabrit.Heroes.Business.Services.Contracts;
using Fabrit.Heroes.Data;
using Fabrit.Heroes.Data.Mapper;
using Fabrit.Heroes.Web.Helpers;
using Fabrit.Heroes.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.IO;

namespace Fabrit.Heroes.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            CurrentEnvironment = environment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (CurrentEnvironment.IsDevelopment())
            {
                // For Development, CORS is required as the backend and frontend have different URLs
                services.AddCors(o => o.AddDefaultPolicy(builder => builder
                    .WithOrigins(Configuration["WebBackendUrl"], Configuration["WebFrontendUrl"])
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                ));
            }

            services.AddControllersWithViews().AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => Configuration.Bind("JwtSettings", options))
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => Configuration.Bind("CookieSettings", options));

            RegisterAppServices(services);
        }

        private void RegisterAppServices(IServiceCollection services)
        {
            // Register DB Context and Config
            services.AddDbContext<HeroesDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("HeroesDbConnection")), ServiceLifetime.Scoped);
            services.AddTransient<HeroesDbConfiguration>();

            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IHeroService, HeroService>();
            services.AddScoped<IPowerService, PowerService>();
            services.AddScoped<IHeroMapper, HeroMapper>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IHashingService, HashingService>();
            services.AddScoped<IEmailService, EmailService>();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.Configure<FormOptions>(o =>
                {
                    o.ValueLengthLimit = int.MaxValue;
                    o.MultipartBodyLengthLimit = int.MaxValue;
                    o.MemoryBufferThreshold = int.MaxValue;
                }
            );

            /*
            * Register services here
            */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var heroesDbConfig = serviceScope.ServiceProvider.GetService<HeroesDbConfiguration>();
                heroesDbConfig.Seed();
            }

            /*
             * Register any middleware here
             */

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Pictures")),
                RequestPath = new PathString("/Pictures")
            });

            app.UseCors();

            app.ConfigureCustomExceptionMiddleware();

            app.UseMiddleware<JwtMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
