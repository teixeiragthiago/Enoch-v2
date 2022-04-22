using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Enoch.CrossCutting
{
    public static class Extensions
    {
        public static byte[] CastObjectToMessageQueueByteArray(this object data) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

        public static IQueryable<T> Paginate<T>(this IQueryable<T> elements, int? page, out int total)
        {
            total = elements.Count();
            return !page.HasValue ? elements.Take(total) : elements.Skip(10 * ((int)page - 1)).Take(10);
        }

        public static List<T> Paginate<T>(this List<T> elements, int? page, out int total)
        {
            total = elements.Count();
            return (List<T>)(!page.HasValue ? elements.Take(total) : elements.Skip(10 * ((int)page - 1)).Take(10));
        }

        public static string FormatCpfCnpj(this long cpfCnpj)
        {
            var convertedInput = $"{cpfCnpj}";
            return convertedInput.PadLeft(cpfCnpj.IsCpf() ? 11 : 14, '0');
        }

        public static bool IsCNPJ(this long cnpj)
        {
            var convertedInput = $"{cnpj}";
            convertedInput = convertedInput.PadLeft(14, '0');

            var first = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var second = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            if (convertedInput.Length != 14)
                return false;
            var cnpjRange = convertedInput.Substring(0, 12);
            var sumValue = 0;
            for (var i = 0; i < 12; i++)
                sumValue += int.Parse(cnpjRange[i].ToString()) * first[i];
            var remnant = (sumValue % 11);
            if (remnant < 2)
                remnant = 0;
            else
                remnant = 11 - remnant;
            var digit = remnant.ToString();
            cnpjRange = cnpjRange + digit;
            sumValue = 0;
            for (var i = 0; i < 13; i++)
                sumValue += int.Parse(cnpjRange[i].ToString()) * second[i];
            remnant = (sumValue % 11);
            if (remnant < 2)
                remnant = 0;
            else
                remnant = 11 - remnant;
            digit += remnant;

            return convertedInput.EndsWith(digit);
        }

        public static bool IsCpf(this long cpf)
        {
            if (cpf.ToString().Length < 11)
                return false;

            var convertedInput = $"{cpf}";

            convertedInput = convertedInput.PadLeft(11, '0');

            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            convertedInput = convertedInput.Trim();
            convertedInput = convertedInput.Replace(".", "").Replace("-", "");
            if (convertedInput.Length != 11)
                return false;
            tempCpf = convertedInput.Substring(0, 9);
            var soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            var resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto;
            return convertedInput.EndsWith(digito);
        }

        public static bool IsValidMail(this string mail)
            => !string.IsNullOrEmpty(mail) && new Regex(@"^([\w-\.]+)@((\[[\d]{1,3}\.[\d]{1,3}\.[\d]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[\d]{1,3})(\]?)$")
                   .IsMatch(mail);

        public static bool IsValidPhone(this string phone)
            => !string.IsNullOrEmpty(phone) && new Regex(@"(\(?\d{2}\)?\s)?(\d{4,5}\-\d{4})")
                   .IsMatch(phone);

        public static string CastCnpj(this long value)
        {
            var cpf = @"/^\d{3}\.\d{3}\.\d{3}\-\d{2}$/";
            var cnpj = @"/^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$/";

            var stringValue = $"{value}";

            if (stringValue.RegexMatch(cpf, out var match))
                return value.ToString();
            else
                stringValue.RegexMatch(cnpj, out match);
            return value.ToString("00000000000000");
        }

        public static bool RegexMatch(this string input, string pattern, out string match)
        {
            var rgx = new Regex(pattern);
            var result = rgx.Match(input);
            match = result.Value;
            return result.Success;
        }



        public static long CastToLong(this string cnpj) => Convert.ToInt64(cnpj);

        public static DateTime DateWithOutHour(this DateTime date) => new DateTime(date.Year, date.Month, date.Day);

        public static DateTime DateWithOnlyHour(this DateTime date) => new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);

        public static bool IsValidEnum<T>(this T value) => value != null && Enum.IsDefined(typeof(T), value);

        public static T DeserializeJson<T>(this HttpContent content)
        {
            return JsonConvert.DeserializeObject<T>(content.ReadAsStringAsync().Result);
        }

        public static string CastByteArraytoBase64Image(byte[] file)
        {
            if (file == null)
                return "";

            var base64String = Convert.ToBase64String(file, 0, file.Length);
            return $"data:image/png;base64,{base64String}";
        }

        public static T DeserializeJsonWithoutToken<T>(this HttpContent content)
        {
            var jsonString = content.DeserializeJson<ApiReturn>();
            var json = JsonConvert.SerializeObject(jsonString.data);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string DeserializeJsonError(this HttpContent content)
        {
            var jsonString = content.DeserializeJson<ApiReturn>();
            return jsonString?.error;
        }

        public static byte[] CastBase64(this string image)
        {
            var index = image.IndexOf("base64", StringComparison.Ordinal);
            if (index == -1)
                return Convert.FromBase64String(image);

            var range = image.Substring(0, index + 7);
            image = image.Replace(range, "");

            return Convert.FromBase64String(image);
        }

        public static IDictionary<string, T> ToDictionary<T>(this object obj)
        {
            if (obj == null)
                return null;

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj))
                dictionary.Add(property.Name, (T)Convert.ChangeType(property.GetValue(obj), typeof(T)));
            return dictionary;
        }

        public static string LowerAndTrim(this string input) => input.ToLower().Trim();

        //public static T FindConfigValue<T>(string key)
        //{
        //    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        //    var basePath = AppDomain.CurrentDomain.BaseDirectory;
        //    var builder = new ConfigurationBuilder()
        //        //.SetBasePath(basePath)
        //        .AddJsonFile("appsettings.json", true, true)
        //        .AddJsonFile($"appsettings.{env}.json", true)
        //        .AddEnvironmentVariables();

        //    var configRoot = builder.Build();

        //    var parameters = configRoot.Providers.Skip(1).FirstOrDefault();
        //    if (parameters != null)
        //    {
        //        parameters.TryGet($"Parameters:{key}", out var value);
        //        return Cast(value, typeof(T));
        //    }

        //    return default(T);
        //}

        public static dynamic Cast(dynamic obj, Type castTo)
        {
            return Convert.ChangeType(obj, castTo);
        }

        public static IQueryable<T> Order<T>(this IEnumerable<T> source, string sortBy = null, string sortDirection = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(sortBy))
                {
                    if (string.IsNullOrEmpty(sortDirection))
                        sortDirection = "asc";

                    var sort = new Sorter<T>();
                    return (IQueryable<T>)sort.Sort(source, sortBy, sortDirection);
                }

                return (IQueryable<T>)source;
            }
            catch (Exception)
            {
                return (IQueryable<T>)source;
            }
        }
    }

    public class ApiReturn
    {
        public string error { get; set; }
        public object data { get; set; }
        public string token { get; set; }
    }

    public class Sorter<T>
    {
        public IEnumerable<T> Sort(IEnumerable<T> source, string sortBy, string sortDirection)
        {
            var param = Expression.Parameter(typeof(T), "item");

            var sortExpression = Expression.Lambda<Func<T, object>>
                (Expression.Convert(Expression.Property(param, sortBy), typeof(object)), param);

            switch (sortDirection.ToLower())
            {
                case "asc":
                    return source.AsQueryable<T>().OrderBy<T, object>(sortExpression);
                default:
                    return source.AsQueryable<T>().OrderByDescending<T, object>(sortExpression);
            }
        }
    }

}

