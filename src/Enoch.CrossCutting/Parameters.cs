using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Enoch.CrossCutting
{
    public class Parameters
    {
        public ParametersValues Data { get; set; }
        public string Secret { get; set; }

        public Parameters()
        {
            Data = GetData();
            Secret = GetSecret();
        }

        private ParametersValues GetData()
        {
            try
            {
                var parameters = new ParametersValues();
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var builder = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env}.json", optional: true)
                    .AddEnvironmentVariables();

                var configRoot = builder.Build();
                configRoot.GetSection("Parameters").Bind(parameters);
                parameters.Environment = env;

                return parameters;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public T FindConfigValue<T>(string key)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true)
                .AddEnvironmentVariables();

            var configRoot = builder.Build();

            var parameters = configRoot.Providers.Skip(1).FirstOrDefault();
            if (parameters != null)
            {
                parameters.TryGet($"Parameters:{key}", out var value);
                return Cast(value, typeof(T));
            }

            return default(T);
        }

        public T FindConfigValueOutParameters<T>(string key)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true)
                .AddEnvironmentVariables();

            var configRoot = builder.Build();

            var parameters = configRoot.Providers.Skip(1).FirstOrDefault();
            if (parameters != null)
            {
                parameters.TryGet(key, out var value);
                return Cast(value, typeof(T));
            }

            return default(T);
        }

        public dynamic Cast(dynamic obj, Type castTo)
        {
            return Convert.ChangeType(obj, castTo);
        }

        public string GetSecret()
        {
            try
            {
                var secret = new AppSettings();
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var builder = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();

                var configRoot = builder.Build();
                configRoot.GetSection("AppSettings").Bind(secret);

                return secret.Secret;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public class ParametersValues
    {
        public Url Url { get; set; }
        public string ConnectionString { get; set; }
        public string AuthConnectionString { get; set; }
        public string MongoConnectionString { get; set; }
        public Mail Mail { get; set; }
        public int Client { get; set; }
        public string AuthIp { get; set; }
        public CorsOrigin CorsOrigin { get; set; }
        public Aws Aws { get; set; }
        public string Environment { get; set; }
        public List<ImageMark> ImageMark { get; set; }
        public List<URLS> Urls { get; set; }
        public int MinLevel { get; set; }
        public DirectoryConfig DirectoryConfig { get; set; }
        public IEnumerable<Mongo> Mongo { get; set; }
    }

    public class Aws
    {
        public string FirstKey { get; set; }
        public string SecondKey { get; set; }
        public string Bucket { get; set; }
    }

    public class Url
    {
        public string Mobile { get; set; }
        public string Web { get; set; }
    }

    public class CorsOrigin
    {
        public string Local { get; set; }
        public string App { get; set; }
    }

    public class Mail
    {
        public string From { get; set; }
        public string To { get; set; }
        public string PassWord { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
    }

    public class ImageMark
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Color { get; set; }
    }

    public class DirectoryConfig
    {
        public string Path { get; set; }
    }

    public class URLS
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Mongo
    {
        public int Id { get; set; }
        public string DataBase { get; set; }
    }

    public class TokenData
    {
        public int Name { get; set; }
        public int Actor { get; set; }
        public string NameIdentifier { get; set; }
    }

}
