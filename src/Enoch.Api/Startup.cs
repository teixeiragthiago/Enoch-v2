using dotenv.net;
using Enoch.Api.Infra;
using Enoch.Api.Infra.HealthChecks;
using Enoch.CrossCutting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Enoch.Api
{
    public class Startup : IStartup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            DotEnv.Load();

            #region Cors

            services.AddSignalR();

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true)
                    .AllowCredentials());
            });

            #endregion

            #region Swagger
            Swagger(services);
            #endregion

            #region Token
            Token(services);
            #endregion

            services.AddHttpClient();

            services.AddControllers();

            services.Register();

            services.ConfigureHealthChecks();
        }



        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("SiteCorsPolicy");

            #region Auth

            app.UseAuthentication();
            app.UseAuthorization();

            #endregion

            #region Swagger

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Morpheus v1.0");
            });

            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            UseHealthChecks(app);
        }

        public void Swagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Enoch",
                        Version = "v 1.0",
                        Description = "Enoch API",
                        Contact = new OpenApiContact
                        {
                            Name = "Enoch",
                            Email = "thiagogabriel76@gmail.com",
                            Url = new Uri("https://github.com/teixeiragthiago"),
                        },
                        License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://github.com/teixeiragthiago") }
                    });
            });
        }

        public void UseHealthChecks(IApplicationBuilder app)
        {
            app.ApiHealthCheck();
        }

        public void Token(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
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

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
        }
    }

    public interface IStartup
    {
        IConfiguration Configuration { get; }
        void Configure(WebApplication app, IWebHostEnvironment environment);
        void ConfigureServices(IServiceCollection services);
    }

    public static class  StartupExtensions 
    {
        public static WebApplicationBuilder UseStartup<TStartup>(this WebApplicationBuilder builder) where TStartup : IStartup
        {
            var startup = Activator.CreateInstance(typeof(TStartup), builder.Configuration) as IStartup;
            if (startup == null) throw new ArgumentException("Classe Startup.cs is invalid!");

            startup.ConfigureServices(builder.Services);

            var app = builder.Build();
            startup.Configure(app, app.Environment);

            app.Run();

            return builder;
        }
    }
}
