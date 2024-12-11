using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GYMaster_API.Entities;
using GYMaster_API.DTO;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GYMaster_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificacionController : ControllerBase
    {
        private readonly GymasterContext _context;
        private IConfiguration _config;

        public VerificacionController(GymasterContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet("Verificar")]
        public async Task<ActionResult> GetVerificacion(string Token)
        {
            Verificacion Verificacion = await _context.Verificacions.Select(
                Data => new Verificacion
                {
                    Usuario = Data.Usuario,
                    Permiso = Data.Permiso,
                    Token = Data.Token
                }
            ).FirstAsync(Request => Request.Token.Equals(Token));

            if (Verificacion == null)
            {
                return NotFound();
            }
            else
            {
                if(Verificacion.Permiso == "Miembro")
                {
                    var Data = await _context.Usuarios.FirstAsync(
                        Request => Request.Usuario1.Equals(Verificacion.Usuario)
                    );
                    Data.IdEstatus = 1;

                    await _context.SaveChangesAsync();

                    return Ok();
                }
                else
                {
                    var Data = await _context.Gimnasios.FirstAsync(
                        Request => Request.Usuario.Equals(Verificacion.Usuario)
                    );
                    Data.IdEstatus = 1;

                    await _context.SaveChangesAsync();

                    return Ok();
                }
            }
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult> PostVerificacion(VerificacionDTO Object)
        {
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Email"] + Object.Usuario));
            var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var TokenData = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                null,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: Credentials);

            var Token = new JwtSecurityTokenHandler().WriteToken(TokenData);

            var Data = new Verificacion()
            {
                Usuario = Object.Usuario,
                Permiso = Object.Permiso,
                Token = Token
            };

            string EmailToken = JsonConvert.SerializeObject(Token);

            _context.Verificacions.Add(Data);
            await _context.SaveChangesAsync();

            return Ok(EmailToken);
        }

        private bool VerificacionExists(int id)
        {
            return (_context.Verificacions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
