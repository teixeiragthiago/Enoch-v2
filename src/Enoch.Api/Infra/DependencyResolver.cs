using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SQS;
using AutoMapper;
using CrossCutting.Email;
using Enoch.CrossCutting;
using Enoch.CrossCutting.AwsS3;
using Enoch.CrossCutting.Notification;
using Enoch.CrossCutting.WebApi;
using Enoch.Domain.Common;
using Enoch.Domain.Services.Auth;
using Enoch.Domain.Services.User;
using Enoch.Domain.Services.User.Queue;
using Enoch.Infra.Base;
using Enoch.Infra.Context;
using Enoch.Infra.User;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Enoch.Api.Infra
{
    public static class DependencyResolver
    {
        public static void Register(this IServiceCollection services)
        {

            #region AutoMapper
            var mappingConfig = new MapperConfiguration(m => 
            {
                m.AddProfile(new AutoMapperProfile());
            });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            services.AddSingleton<Parameters>();
            services.AddSingleton<Email>();
            services.AddScoped<INotification, Notification>();

            services.AddSingleton<HttpClient>();
            services.AddHttpClient();
            services.AddScoped<IWebApi, WebApi>();
            services.AddSingleton<HttpClient>();

            #region DbContext
            services.AddDbContext<DataContext>();
            #endregion

            #region Factories, Repositories, Services and Queues
            Factories(services);
            Services(services);
            Repositories(services);
            Queues(services);
            #endregion

            #region AwsSQS
            AwsSqs(services);
            #endregion

            #region AwsS3   
            AwsS3(services);
            #endregion

        }

        private static void AwsS3(this IServiceCollection services)
        {
            services.AddAWSService<IAmazonS3>();
            services.AddSingleton<AwsStorage>();
        }

        private static void AwsSqs(this IServiceCollection services)
        {
            var cred = new BasicAWSCredentials(Environment.GetEnvironmentVariable("AWS_ACCESSKEY"), Environment.GetEnvironmentVariable("AWS_SECRET"));

            services.AddDefaultAWSOptions(new AWSOptions
            {
                Region = RegionEndpoint.SAEast1,
                Profile = Environment.GetEnvironmentVariable("AWS_PROFILE"),
                Credentials = cred,
            });

            services.AddAWSService<IAmazonSQS>();
        }

        private static void Factories(IServiceCollection services)
        {
            services.AddScoped<IUserFactory, UserFactory>();
            services.AddScoped<DapperBaseRepository>();
        }

        private static void Queues(IServiceCollection services)
        {
            services.AddScoped<IUserQueue, UserQueue>();
        }

        private static void Services(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
        }

        private static void Repositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
