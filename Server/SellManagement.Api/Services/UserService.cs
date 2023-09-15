using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SellManagement.Api.Entities;
using SellManagement.Api.Helpers;
using SellManagement.Api.Models;

namespace SellManagement.Api.Services
{
    public class UserService:IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly SellManagementContext _context;

        public UserService(SellManagementContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            try
            {
                var user = _context.TblUsers.SingleOrDefault(x => x.LoginId == model.LoginId);

                // return null if user not found
                if (user == null) return null;

                //Check password hashed
                var isPasswordMatched = VerifyPassword(model.Password, user.StoredSalt, user.Password);
                if (!isPasswordMatched) return null;

                // authentication successful so generate jwt token
                var token = GenerateJwtToken(user);

                return new AuthenticateResponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    UserRole = user.UserRole,
                    Token = token
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string GenerateJwtToken(TblUser user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Functions.User GetUserById(int id)
        {
            return _context.TblUsers.Where(x=>x.Id == id).Select(x=> new Functions.User
            {
                Id = x.Id,
                UserName= x.UserName,
                UserRole= x.UserRole,
            }).SingleOrDefault();
        }

        public HashSalt EncryptPassword(string password)
        {
            byte[] salt = new byte[128 / 8]; // Generate a 128-bit salt using a secure PRNG
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return new HashSalt { Hash = encryptedPassw, Salt = salt };
        }

        public bool VerifyPassword(string enteredPassword, byte[] salt, string storedPassword)
        {
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return encryptedPassw == storedPassword;
        }
    }
}
