using DevIO.Api.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.Configuration
{
    public class IdentityManager : IidentityManager
    {
        public readonly SignInManager<IdentityUser> _signInManager;
        public readonly UserManager<IdentityUser> _userManager;
        public IdentityManager(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
    }
}
