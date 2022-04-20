using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.SQS;
using dotenv.net;
using Enoch.Api.Infra;
using Enoch.Api.Infra.HealthChecks;
using Enoch.CrossCutting;
using Enoch.CrossCutting.AwsSQS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Enoch.Api
{
    public class Startup
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

            //var cred = new BasicAWSCredentials(Environment.GetEnvironmentVariable("AWS_ACCESSKEY"), Environment.GetEnvironmentVariable("AWS_SECRET"));

            //services.AddDefaultAWSOptions(new AWSOptions
            //{
            //    Region = RegionEndpoint.SAEast1,
            //    Profile = Environment.GetEnvironmentVariable("AWS_PROFILE"),
            //    Credentials = cred,
            //});

            //services.AddAWSService<IAmazonSQS>();

            services.AddHttpClient();

            services.AddControllers();

            services.Register();

            services.ConfigureHealthChecks();
        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
}
