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
using NuGet.Protocol;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GYMaster_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsuariosController : ControllerBase
    {
        private readonly GymasterContext _context;
        private IConfiguration _config;

        public UsuariosController(GymasterContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("Validar")]
        public async Task<ActionResult> ValidarUsuario(CredencialesDTO Object)
        {
            Usuario Usuario = await _context.Usuarios.Select(
                Data => new Usuario
                {
                    Nombres = Data.Nombres,
                    Apellidos = Data.Apellidos,
                    FechaNacimiento = Data.FechaNacimiento,
                    Sexo = Data.Sexo,
                    Estatura = Data.Estatura,
                    Peso = Data.Peso,
                    Telefono = Data.Telefono,
                    Correo = Data.Correo,
                    Usuario1 = Data.Usuario1,
                    Contrasena = Data.Contrasena,
                    IdGimnasio = Data.IdGimnasio,
                    FechaRegistro = Data.FechaRegistro,
                    IdEstatus = Data.IdEstatus,
                    CodigoTarjeta = Data.CodigoTarjeta
                }
            ).FirstAsync(Request => Request.Usuario1.Equals(Object.Usuario));

            if (Usuario.Contrasena != Object.Contrasena)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            else if (Usuario.IdEstatus != 1)
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

        [HttpGet("Catalogo")]
        public async Task<ActionResult<List<UsuariosDTO>>> GetUsuariosGimnasio(string UsuarioGimnasios)
        {
            var List = await _context.Usuarios.Where(Data => Data.IdGimnasio.Equals(
                    _context.Gimnasios.First(
                        Request => Request.Usuario.Equals(UsuarioGimnasios)
                    ).Id
                )
            ).Select(
                  Data => new UsuariosDTO
                  {
                      Nombres = Data.Nombres,
                      Apellidos = Data.Apellidos,
                      FechaNacimiento = Data.FechaNacimiento,
                      Sexo = Data.Sexo,
                      Estatura = Data.Estatura,
                      Peso = Data.Peso,
                      Telefono = Data.Telefono,
                      Correo = Data.Correo,
                      Usuario1 = Data.Usuario1,
                      Contrasena = "",
                      UsuarioGimnasios = UsuarioGimnasios,
                      FechaRegistro = Data.FechaRegistro,
                      CodigoTarjeta = ""
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
        public async Task<ActionResult<UsuariosDTO>> GetUsuarios(string UsuarioUsuarios)
        {
            UsuariosDTO Usuario = await _context.Usuarios.Select(
                  Data => new UsuariosDTO
                  {
                      Nombres = Data.Nombres,
                      Apellidos = Data.Apellidos,
                      FechaNacimiento = Data.FechaNacimiento,
                      Sexo = Data.Sexo,
                      Estatura = Data.Estatura,
                      Peso = Data.Peso,
                      Telefono = Data.Telefono,
                      Correo = Data.Correo,
                      Usuario1 = Data.Usuario1,
                      Contrasena = Data.Contrasena,
                      UsuarioGimnasios = _context.Gimnasios.First(Request => Request.Id.Equals(Data.IdGimnasio)).Usuario,
                      FechaRegistro = Data.FechaRegistro,
                      CodigoTarjeta = Data.CodigoTarjeta
                  }
            ).FirstAsync(Request => Request.Usuario1.Equals(UsuarioUsuarios));

            if (Usuario == null)
            {
                return NotFound();
            }
            else
            {
                return Usuario;
            }
        }

        [HttpGet("BuscarCorreo")]
        public async Task<ActionResult<UsuariosDTO>> GetUsuariosCorreo(string CorreoUsuarios)
        {
            UsuariosDTO Usuario = await _context.Usuarios.Select(
                  Data => new UsuariosDTO
                  {
                      Nombres = Data.Nombres,
                      Apellidos = Data.Apellidos,
                      FechaNacimiento = Data.FechaNacimiento,
                      Sexo = Data.Sexo,
                      Estatura = Data.Estatura,
                      Peso = Data.Peso,
                      Telefono = Data.Telefono,
                      Correo = Data.Correo,
                      Usuario1 = Data.Usuario1,
                      Contrasena = Data.Contrasena,
                      UsuarioGimnasios = _context.Gimnasios.First(Request => Request.Id.Equals(Data.IdGimnasio)).Usuario,
                      FechaRegistro = Data.FechaRegistro,
                      CodigoTarjeta = Data.CodigoTarjeta
                  }
            ).FirstAsync(Request => Request.Correo.Equals(CorreoUsuarios));

            if (Usuario == null)
            {
                return NotFound();
            }
            else
            {
                return Usuario;
            }
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult> PostUsuario(UsuariosDTO Object)
        {
            var Data = new Usuario()
            {
                Nombres = Object.Nombres,
                Apellidos = Object.Apellidos,
                FechaNacimiento = Object.FechaNacimiento,
                Sexo = Object.Sexo,
                Estatura = Object.Estatura,
                Peso = Object.Peso,
                Telefono = Object.Telefono,
                Correo = Object.Correo,
                Usuario1 = Object.Usuario1,
                Contrasena = Object.Contrasena,
                IdGimnasio = 1,
                FechaRegistro = Object.FechaRegistro,
                IdEstatus = 2,
                CodigoTarjeta = "",
            };

            _context.Usuarios.Add(Data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("Modificar")]
        public async Task<ActionResult> PutUsuario(UsuariosDTO Object)
        {
            var Data = await _context.Usuarios.FirstAsync(
                 Request => Request.Usuario1.Equals(Object.Usuario1)
             );
            Data.Nombres = Object.Nombres;
            Data.Apellidos = Object.Apellidos;
            Data.Telefono = Object.Telefono;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("Actualizar")]
        public async Task<ActionResult> PatchUsuario(CredencialesDTO Object)
        {
            var Data = await _context.Usuarios.FirstAsync(
                Request => Request.Usuario1.Equals(Object.Usuario)
            );
            Data.Contrasena = Object.Contrasena;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("Afiliar")]
        public async Task<ActionResult> PatchAfiliarUsuario(string UsuarioUsuarios, string UsuarioGimnasios)
        {
            var Data = await _context.Usuarios.FirstAsync(
                Request => Request.Usuario1.Equals(UsuarioUsuarios)
            );

            Data.IdGimnasio = _context.Gimnasios.First(Request => Request.Usuario.Equals(UsuarioGimnasios)).Id;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("RegistroTarjeta")]
        public async Task<HttpStatusCode> PatchTarjetaUsuario(string UsuarioUsuarios, string CodigoNFC)
        {
            var Data = await _context.Usuarios.FirstAsync(
                Request => Request.Usuario1.Equals(UsuarioUsuarios)
            );

            Data.CodigoTarjeta = CodigoNFC;

            await _context.SaveChangesAsync();

            return HttpStatusCode.OK;
        }

        private bool UsuarioExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
