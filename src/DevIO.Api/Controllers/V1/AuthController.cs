using DevIO.Api.Configuration;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers.V1
{
    [ApiVersion("1.0")]    
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : MainController
    {
        public readonly IidentityManager _identityManager;
        private readonly ILogger _logger;
        public AuthController(INotificador notificador,
                              IUser user,
                              IidentityManager identityManager,
                              ILogger<AuthController> logger) : base(notificador, user)
        {
            _identityManager = identityManager;
            _logger = logger;
        }

        [HttpPost("registrar-usuario")]
        public async Task<ActionResult> Registrar([FromBody] UserRegisterViewModel UserRegister)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

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
                _logger.LogInformation($"Usuario {UserLogin.Email} logado com sucesso");
                return CustomResponse(await _identityManager.GenerateJwt(UserLogin.Email));
            }

            NotificarErro("Usuario ou senha incorreta");
            return CustomResponse(UserLogin);
        }
    }
}
