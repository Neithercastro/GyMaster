using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GYMaster_API.Entities;
using System.Net;
using GYMaster_API.DTO;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GYMaster_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GimnasiosController : ControllerBase
    {
        private readonly GymasterContext _context;
        private IConfiguration _config;

        public GimnasiosController(GymasterContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("Validar")]
        public async Task<ActionResult> ValidarGimnasio(CredencialesDTO Object)
        {
            Gimnasio Gimnasio = await _context.Gimnasios.Select(
                  Data => new Gimnasio
                  {
                      Nombre = Data.Nombre,
                      Correo = Data.Correo,
                      Usuario = Data.Usuario,
                      Contrasena = Data.Contrasena,
                      IdEstatus = Data.IdEstatus
                  }
            ).FirstAsync(Request => Request.Usuario.Equals(Object.Usuario));

            if (Gimnasio.Contrasena != Object.Contrasena)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            else if (Gimnasio.IdEstatus == 2)
            {
                object Problema = JsonConvert.SerializeObject("Sin confirmar");

                return Accepted(Problema);
            }
            else
            {
                var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

                var TokenData = new JwtSecurityToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    null,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: Credentials);

                var Token = new JwtSecurityTokenHandler().WriteToken(TokenData);

                string SessionToken = JsonConvert.SerializeObject(Token);

                return Ok(SessionToken);
            }
        }

        [HttpGet("Mostrar")]
        public async Task<ActionResult<List<GimnasiosDTO>>> GetGimnasio()
        {
            var List = await _context.Gimnasios.Where(Request => Request.IdEstatus == 1)
            .Select(
                  Data => new GimnasiosDTO
                  {
                      Nombre = Data.Nombre,
                      Correo = Data.Correo,
                      Usuario = Data.Usuario,
                      Contrasena = ""
                  }
            ).ToListAsync();

            if (List == null)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }

        [HttpGet("Buscar")]
        public async Task<ActionResult<GimnasiosDTO>> GetGimnasio(string UsuarioGimnasios)
        {
            GimnasiosDTO Gimnasio = await _context.Gimnasios.Select(
                  Data => new GimnasiosDTO
                  {
                      Nombre = Data.Nombre,
                      Correo = Data.Correo,
                      Usuario = Data.Usuario,
                      Contrasena = Data.Contrasena,
                  }
            ).FirstAsync(Request => Request.Usuario.Equals(UsuarioGimnasios));

            if (Gimnasio == null)
            {
                return NotFound();
            }
            else
            {
                return Gimnasio;
            }
        }

        [HttpGet("BuscarCorreo")]
        public async Task<ActionResult<GimnasiosDTO>> GetGimnasioCorreo(string CorreoGimnasios)
        {
            GimnasiosDTO Gimnasio = await _context.Gimnasios.Select(
                  Data => new GimnasiosDTO
                  {
                      Nombre = Data.Nombre,
                      Correo = Data.Correo,
                      Usuario = Data.Usuario,
                      Contrasena = Data.Contrasena,
                  }
            ).FirstAsync(Request => Request.Correo.Equals(CorreoGimnasios));

            if (Gimnasio == null)
            {
                return NotFound();
            }
            else
            {
                return Gimnasio;
            }
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult> PostGimnasio(GimnasiosDTO Object)
        {
            var Data = new Gimnasio() {
                Nombre = Object.Nombre,
                Correo = Object.Correo,
                Usuario = Object.Usuario,
                Contrasena = Object.Contrasena,
                IdEstatus = 2
            };

            _context.Gimnasios.Add(Data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("Modificar")]
        public async Task<ActionResult> PutGimnasio(GimnasiosDTO Object)
        {
            var Data = await _context.Gimnasios.FirstAsync(
                Request => Request.Usuario.Equals(Object.Usuario)
            );

            Data.Nombre = Object.Nombre;
            
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("Actualizar")]
        public async Task<ActionResult> PatchGimnasio(CredencialesDTO Object)
        {
            var Data = await _context.Gimnasios.FirstAsync(
                Request => Request.Usuario.Equals(Object.Usuario)
            );
            Data.Contrasena = Object.Contrasena;

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool GimnasioExists(int id)
        {
            return (_context.Gimnasios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}