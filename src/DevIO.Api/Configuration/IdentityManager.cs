using DevIO.Api.Extensions;
using DevIO.Api.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Api.Configuration
{
    public class IdentityManager : IidentityManager
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        public IdentityManager(
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager,
            IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }


        //registra e faz login
        public async Task<IEnumerable<string>> RegisterUser(UserRegisterViewModel userRegister)
        {            
            var user = new IdentityUser
            {
                UserName = userRegister.Email,
                Email = userRegister.Email,
                EmailConfirmed = true
            };

            var identityResult = await _userManager.CreateAsync(user, userRegister.Password);

            if (identityResult.Succeeded) 
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return null;
            }
            else
            {
                return identityResult.Errors.Select(e => e.Description);                
            }            
        }

        public async Task<bool> Login(UserLoginViewModel UserLogin)
        {
            var result = await _signInManager.PasswordSignInAsync(UserLogin.Email, UserLogin.Password, false, true);

            if (result.Succeeded) 
            {
                return true;
            }
            else if (result.IsLockedOut)
            {
                throw new ArgumentException($"User temporarily blocked for too many attempts");
            }

            return false;
        }

        public string GenerateJwt()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.ValidOn,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),                
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }
    }
}
