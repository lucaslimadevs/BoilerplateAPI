using DevIO.Api.Configuration;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : MainController
    {
        public readonly IidentityManager _identityManager;
        public AuthController(INotificador notificador, IidentityManager identityManager) : base(notificador)
        {
            _identityManager = identityManager;
        }

        [HttpPost("registrar-usuario")]
        public async Task<ActionResult> Registrar([FromBody] UserRegisterViewModel UserRegister)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _identityManager.RegisterUser(UserRegister);

            if (result is null)
            {
                return CustomResponse(await _identityManager.GenerateJwt(UserRegister.Email));
            }

            foreach (var error in result)
            {
                NotificarErro(error);
            }
            
            return CustomResponse(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginViewModel UserLogin)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var logged = await _identityManager.Login(UserLogin);

            if (logged)
            {
                return CustomResponse(await _identityManager.GenerateJwt(UserLogin.Email));
            }

            NotificarErro("Incorrect username or password");
            return CustomResponse(UserLogin);
        }
    }
}
