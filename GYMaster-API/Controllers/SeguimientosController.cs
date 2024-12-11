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

namespace GYMaster_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguimientosController : ControllerBase
    {
        private readonly GymasterContext _context;

        public SeguimientosController(GymasterContext context)
        {
            _context = context;
        }

        [HttpGet("Catalogo")]
        public async Task<ActionResult<List<SeguimientoDTO>>> GetSeguimiento(string UsuarioUsuarios)
        {
            var List = await _context.Seguimientos.Where(
                  Request => Request.IdUsuario.Equals(
                        _context.Usuarios.First(
                                Data => Data.Usuario1.Equals(UsuarioUsuarios)
                            ).Id
                      )
              ).Select(
                  Data => new SeguimientoDTO
                  {
                      UsuarioUsuarios = UsuarioUsuarios,
                      Sesion = Data.Sesion,
                      Imc = Data.Imc,
                      Fecha = Data.Fecha
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

        [HttpPost("Registrar")]
        public async Task<ActionResult> PostSeguimiento(int Peso, SeguimientoDTO Object)
        {
            var Data = new Seguimiento()
            {
                IdUsuario = _context.Usuarios.First(Request => Request.Usuario1.Equals(Object.UsuarioUsuarios)).Id,
                Sesion = Object.Sesion,
                Imc = Object.Imc,
                Fecha = Object.Fecha
            };
            _context.Seguimientos.Add(Data);

            var Usuario = await _context.Usuarios.FirstAsync(
                Request => Request.Usuario1.Equals(Object.UsuarioUsuarios)
            );

            Usuario.Peso = Peso;

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool SeguimientoExists(int id)
        {
            return (_context.Seguimientos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
