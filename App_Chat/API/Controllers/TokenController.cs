using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using API.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly TokenProvider token;

        public TokenController(TokenProvider token)
        {
            this.token = token;
        }

        [HttpPost]
        public IActionResult Post(string username, string password)
        {
            if (username != null && password != null)
            {
                if (username == password) //TODO... Busqueda en base de datos
                {
                    return new ObjectResult(token.GenerateToken(username));
                }
            }

            //Si llega hasta aqui, algo salio mal...
            return BadRequest();
        }
    }
}