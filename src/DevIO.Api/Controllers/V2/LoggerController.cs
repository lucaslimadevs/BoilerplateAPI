using DevIO.Business.Intefaces;
using Elmah.Io.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DevIO.Api.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class LoggerController : MainController
    {
        private readonly ILogger _logger;

        public LoggerController(INotificador notificador, IUser appUser, ILogger<LoggerController> logger) : base(notificador, appUser)
        {
            _logger = logger;
        }

        [HttpGet]
        public string ListarLogs()
        {
            try
            {
                var i = 0;
                var result = 42 / i;
            }
            catch (DivideByZeroException e)
            {
                e.Ship(HttpContext); //enviar exception para Elmah Logging
            }

            _logger.LogTrace("Log de Trace"); //obsoleto
            _logger.LogDebug("Log de Debug");
            _logger.LogInformation("Log de Informaçao");
            _logger.LogWarning("Log de Aviso");
            _logger.LogError("Log de Erro");
            _logger.LogCritical("Log de Problema critico");

            return "Logs retornados! Verifique o console";
        }
    }
}
