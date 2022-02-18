using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Enoch.CrossCutting
{
    public static class Encryption
    {
        private const string AllowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789abcdefghijkmnopqrstuvwxyz!@$?-";
        private const string Key = "DFARSDHD4D4A4DS1F2F4A1JK4Y4R4E4W4U4I4YH4S4S4A4DF";

        public static string CreateChar(string input)
        {
            var finalPassWord = string.Empty;
            for (var i = 0; i <= input.Length - 1; i++)
            {
                var index = AllowedChars.IndexOf(input[i]);
                index += 10;
                if (index > AllowedChars.Length - 1)
                    index = index - (AllowedChars.Length - 1);

                finalPassWord += AllowedChars[index];
            }

            return finalPassWord;
        }

        public static string TrueCharValue(string input)
        {
            var originalPassWord = string.Empty;
            for (var i = 0; i <= input.Length - 1; i++)
            {
                var index = AllowedChars.IndexOf(input[i]);
                index -= 10;
                if (index - 1 < 0)
                    index = (AllowedChars.Length) + (index - 1);

                originalPassWord += AllowedChars[index];
            }

            return originalPassWord;
        }

        public static string CreateDefaultPassWord(int stringLength)
        {
            var rd = new Random();
            var chars = new char[stringLength];
            for (var i = 0; i < stringLength; i++)
                chars[i] = AllowedChars[rd.Next(0, AllowedChars.Length)];

            return new string(chars);
        }

        public static string Encrypt(string input)
        {
            var bytesBuff = Encoding.Unicode.GetBytes(input);
            using (var aes = Aes.Create())
            {
                using (var crypto = new Rfc2898DeriveBytes(Key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }))
                {
                    aes.Key = crypto.GetBytes(32);
                    aes.IV = crypto.GetBytes(16);
                    using (var mStream = new MemoryStream())
                    {
                        using (var cStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cStream.Write(bytesBuff, 0, bytesBuff.Length);
                            cStream.Dispose();
                        }
                        input = Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            return input;
        }

        public static string Decrypt(string cryptoInput)
        {
            var bytesBuff = Convert.FromBase64String(cryptoInput);
            using (var aes = Aes.Create())
            {
                using (var crypto = new Rfc2898DeriveBytes(Key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }))
                {
                    aes.Key = crypto.GetBytes(32);
                    aes.IV = crypto.GetBytes(16);
                    using (var mStream = new MemoryStream())
                    {
                        using (var cStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cStream.Write(bytesBuff, 0, bytesBuff.Length);
                            cStream.Dispose();
                        }
                        cryptoInput = Encoding.Unicode.GetString(mStream.ToArray());
                    }
                }

            }
            return cryptoInput;
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("PassWord informado é inválido!");

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (storedHash.Length != 64)
                return false;
            if (storedSalt.Length != 128)
                return false;

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                if (computedHash.Where((t, i) => t != storedHash[i]).Any())
                    return false;
            }

            return true;
        }

        public static string CreateToken(TokenData tknData)
        {
            var parameters = new Parameters();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(parameters.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, Encrypt(tknData.Name.ToString())),
                    new Claim(ClaimTypes.Actor, Encrypt(tknData.Actor.ToString())),
                    new Claim(ClaimTypes.NameIdentifier, Encrypt(tknData.NameIdentifier)),
                }),

                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //public static Token Token(this string token)
        //{
        //    if (string.IsNullOrEmpty(token))
        //        return default;

        //    token = token.Replace("Bearer ", "").Replace("Bearer", "");
        //    var handler = new JwtSecurityTokenHandler();
        //    var claims = handler.ReadJwtToken(token).Claims.ToList();

        //    if (string.IsNullOrEmpty(token))
        //        return null;

        //    var dateNow = DateTime.Now;
        //    var dateBirth = Convert.ToDateTime(claims[4].Value);
        //    return new Token
        //    {
        //        Name = Decrypt(claims[0].Value),
        //        Actor = Decrypt(claims[1].Value),
        //        NameIdentifier = Decrypt(claims[2].Value),
        //        Authentication = claims[3].Value,
        //        IntervalToken = dateNow.Subtract(dateBirth).TotalMinutes
        //    };
        //}

    }

    public class Token
    {
        /// <summary>
        /// User Id in application
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Employ Id ex: CNPJ or Local control id
        /// </summary>
        public string Actor { get; set; }
        /// <summary>
        /// User Name
        /// </summary>
        public string NameIdentifier { get; set; }
        /// <summary>
        /// Auth Token
        /// </summary>
        public string Authentication { get; set; }

        /// <summary>
        /// Time interval of token creating and now
        /// </summary>
        public double IntervalToken { get; set; }

        /// <summary>
        /// Date of the token birth!
        /// </summary>
        public DateTime DateOfBirth { get; set; }
    }
}
