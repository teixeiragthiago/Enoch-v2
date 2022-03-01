using AutoMapper;
using CrossCutting.Email;
using Enoch.CrossCutting;
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
using System.Net.Http;

namespace Enoch.Api.Infra
{
    public static class DependencyResolver
    {
        public static void Register(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(m => 
            {
                m.AddProfile(new AutoMapperProfile());
            });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSingleton<Parameters>();
            services.AddSingleton<Email>();

            services.AddScoped<INotification, Notification>();

            services.AddSingleton<HttpClient>();
            services.AddHttpClient();
            services.AddScoped<IWebApi, WebApi>();
            services.AddSingleton<HttpClient>();
            services.AddDbContext<DataContext>();

            Factories(services);
            Services(services);
            Repositories(services);
            Queues(services);
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
