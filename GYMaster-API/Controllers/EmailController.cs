using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GYMaster_API.Services;
using GYMaster_API.DTO;
using NuGet.Protocol;
using Microsoft.EntityFrameworkCore;
using GYMaster_API.Entities;

namespace GYMaster_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly GymasterContext _context;

        public EmailController(IEmailService emailService, GymasterContext context)
        {
            _emailService = emailService;
            _context = context;
        }

        [HttpPost("Confirmacion")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDTO Request)
        {
            RecoverPasswordDTO Correo = await ObtenerEmail(Request.Usuario);

            if (Correo == null)
            {
                return NotFound();
            }

            // Verificar si el correo electrónico existe en la base de datos
            CredencialesDTO Credenciales = await ObtenerUsuario(Correo.Email);

            if (Credenciales == null)
            {
                return NotFound();
            }

            Verificacion Verificacion = await _context.Verificacions.Select(
                Data => new Verificacion
                {
                    Usuario = Data.Usuario,
                    Permiso = Data.Permiso,
                    Token = Data.Token
                }
            ).FirstAsync(
                Data => Data.Usuario.Equals(
                    Request.Usuario
                )
            );

            if (Verificacion == null)
            {
                Verificacion = await _context.Verificacions.Select(
                    Data => new Verificacion
                    {
                        Usuario = Data.Usuario,
                        Permiso = Data.Permiso,
                        Token = Data.Token
                    }
                ).FirstAsync(
                    Data => Data.Usuario.Equals(
                        Request.Usuario
                    )
                );
            }

            if (Verificacion == null)
            {
                return NotFound();
            }

            string Token = "http://localhost:4200/ConfirmarCorreo?Token=" + Verificacion.Token;

            _emailService.SendConfirmationEmail(Correo.Email, Token, Credenciales);

            return Ok();
        }

        [HttpPost("Recuperacion")]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordDTO Request)
        {
            // Verificar si el correo electrónico existe en la base de datos
            CredencialesDTO Credenciales = await ObtenerCredenciales(Request.Email);

            if (Credenciales == null)
            {
                return NotFound();
            }

            _emailService.SendRecoveryEmail(Request.Email, Credenciales);

            return Ok();
        }

        [HttpPost("Recibo")]
        public async Task<IActionResult> PurchaseReceipt(PurchaseReceiptDTO Request)
        {
            // Verificar si el correo electrónico existe en la base de datos
            CredencialesDTO Credenciales = await ObtenerUsuario(Request.Email);

            if (Credenciales == null)
            {
                return NotFound();
            }

            _emailService.SendReceiptEmail(Request.Email, Request.Productos, Credenciales);

            return Ok();
        }

        private async Task<CredencialesDTO> ObtenerUsuario(string Email)
        {
            var User = _context.Usuarios.FirstOrDefault(Request => Request.Correo.Equals(Email))?.Nombres;

            if (User != null)
            {
                CredencialesDTO Credenciales = await _context.Usuarios.Where(Request => Request.Correo.Equals(Email))
                .Select(
                    Data => new CredencialesDTO
                    {
                        Usuario = Data.Nombres,
                        Contrasena = "Miembro"
                    }
                ).FirstAsync();

                return Credenciales;
            }
            else
            {
                CredencialesDTO Credenciales = await _context.Gimnasios.Where(Request => Request.Correo.Equals(Email))
                .Select(
                    Data => new CredencialesDTO
                    {
                        Usuario = Data.Nombre,
                        Contrasena = "Gimnasio"
                    }
                ).FirstAsync();

                return Credenciales;
            }
        }

        private async Task<CredencialesDTO> ObtenerCredenciales(string Email)
        {
            var User = _context.Usuarios.FirstOrDefault(Request => Request.Correo.Equals(Email))?.Nombres;

            if (User != null)
            {
                CredencialesDTO Credenciales = await _context.Usuarios.Where(Request => Request.Correo.Equals(Email))
                .Select(
                    Data => new CredencialesDTO
                    {
                        Usuario = Data.Nombres,
                        Contrasena = Data.Contrasena
                    }
                ).FirstAsync();

                return Credenciales;
            }
            else
            {
                CredencialesDTO Credenciales = await _context.Gimnasios.Where(Request => Request.Correo.Equals(Email))
                .Select(
                    Data => new CredencialesDTO
                    {
                        Usuario = Data.Nombre,
                        Contrasena = Data.Contrasena
                    }
                ).FirstAsync();

                return Credenciales;
            }
        }
        private async Task<RecoverPasswordDTO> ObtenerEmail(string Usuario)
        {
            var User = _context.Usuarios.FirstOrDefault(Request => Request.Usuario1.Equals(Usuario))?.Correo;

            if (User != null)
            {
                RecoverPasswordDTO Email = await _context.Usuarios.Where(Request => Request.Usuario1.Equals(Usuario))
                .Select(
                    Data => new RecoverPasswordDTO
                    {
                        Email = Data.Correo
                    }
                ).FirstAsync();

                return Email;
            }
            else
            {
                RecoverPasswordDTO Email = await _context.Gimnasios.Where(Request => Request.Usuario.Equals(Usuario))
                .Select(
                    Data => new RecoverPasswordDTO
                    {
                        Email = Data.Correo
                    }
                ).FirstAsync();

                return Email;
            }
        }
    }
}