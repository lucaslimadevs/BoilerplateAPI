using DevIO.Api.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Api.Configuration
{
    public interface IidentityManager
    {
        Task<IEnumerable<string>> RegisterUser(UserRegisterViewModel userRegister);
        Task<bool> Login(UserLoginViewModel UserLogin);
        Task<string> GenerateJwt(string email);
    }
}
