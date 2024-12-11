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

namespace GYMaster_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuscripcionesController : ControllerBase
    {
        private readonly GymasterContext _context;

        public SuscripcionesController(GymasterContext context)
        {
            _context = context;
        }

        [HttpGet("Buscar")]
        public async Task<ActionResult<SuscripcionesDTO>> GetSuscripcione(string UsuarioUsuarios)
        {
            SuscripcionesDTO Suscripcion = await _context.Suscripciones.Where(
                Request => Request.IdUsuario.Equals(
                    _context.Usuarios.First(
                        Data => Data.Usuario1.Equals(UsuarioUsuarios)
                    ).Id
                )
            ).Select(
                Data => new SuscripcionesDTO
                {
                    UsuarioUsuarios = UsuarioUsuarios,
                    UsuarioGimnasios = _context.Gimnasios.First(Gimnasio => Gimnasio.Id.Equals(
                        _context.Membresias.First(Request => Request.Id.Equals(Data.IdMembresia)).IdGimnasio
                    )).Usuario,
                    TipoMembresia = _context.Membresias.First(Request => Request.Id.Equals(Data.IdMembresia)).Tipo,
                    FechaRenovacion = Data.FechaRenovacion
                }
            ).FirstAsync();

            if (Suscripcion == null)
            {
                return NotFound();
            }
            else
            {
                return Suscripcion;
            }
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult> PostSuscripcione(SuscripcionesDTO Object)
        {
            var Data = new Suscripcione()
            {
                IdUsuario = _context.Usuarios.First(Request => Request.Usuario1.Equals(Object.UsuarioUsuarios)).Id,
                IdMembresia = _context.Membresias.First(
                    Request => Request.Tipo.Equals(Object.TipoMembresia) && Request.IdGimnasio.Equals(
                        _context.Gimnasios.First(Data => Data.Usuario.Equals(Object.UsuarioGimnasios)).Id
                    )
                ).Id,
                FechaRenovacion = Object.FechaRenovacion
            };
            _context.Suscripciones.Add(Data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("Modificar")]
        public async Task<ActionResult> PutSuscripcione(SuscripcionesDTO Object)
        {
            var Data = await _context.Suscripciones.FirstAsync(
                Request => Request.IdUsuario.Equals(
                    _context.Usuarios.First(
                        Data => Data.Usuario1.Equals(Object.UsuarioUsuarios)
                    ).Id
                )
            );
            Data.IdUsuario = Data.IdUsuario;
            Data.IdMembresia = _context.Membresias.First(
                    Request => Request.Tipo.Equals(Object.TipoMembresia) && Request.IdGimnasio.Equals(
                        _context.Gimnasios.First(Data => Data.Usuario.Equals(Object.UsuarioGimnasios)).Id
                    )
                ).Id;
            Data.FechaRenovacion = Object.FechaRenovacion;

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool SuscripcioneExists(int id)
        {
            return (_context.Suscripciones?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
