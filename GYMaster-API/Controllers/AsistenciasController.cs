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
    public class AsistenciasController : ControllerBase
    {
        private readonly GymasterContext _context;

        public AsistenciasController(GymasterContext context)
        {
            _context = context;
        }

        [HttpGet("Buscar")]
        public async Task<ActionResult<List<AsistenciasDTO>>> GetAsistenciasUsuario(string UsuarioUsuarios)
        {
            var List = await _context.Asistencias.Where(
                Request => Request.IdUsuario.Equals(
                        _context.Usuarios.First(
                                Data => Data.Usuario1.Equals(UsuarioUsuarios)
                            ).Id
                    )
            )
            .Select(
                  Data => new AsistenciasDTO
                  {
                      UsuarioUsuarios = UsuarioUsuarios,
                      Fecha = Data.Fecha,
                      Hora = Data.Hora
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

        [HttpGet("Catalogo")]
        public async Task<ActionResult<List<AsistenciasDTO>>> GetAsistencias(string UsuarioGimnasios)
        {
            var List = await _context.Usuarios.Where(
                Request => Request.IdGimnasio.Equals(
                        _context.Gimnasios.First(
                                Data => Data.Usuario.Equals(UsuarioGimnasios)
                            ).Id
                    )
            ).Join(
                _context.Asistencias,
                Usuario => Usuario.Id,
                Asistencia => Asistencia.IdUsuario,
                (TableUsuario, TableAsistencia) => new AsistenciasDTO
                {
                    UsuarioUsuarios = TableUsuario.Nombres + " " + TableUsuario.Apellidos,
                    Fecha = TableAsistencia.Fecha,
                    Hora = TableAsistencia.Hora
                }
             ).ToListAsync();

            if (List == null)
            {
                return NotFound();
            }
            else {
                return List;
            }
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult> PostAsistencia(AsistenciasDTO Object)
        {
            var Data = new Asistencia()
            {
               IdUsuario = _context.Usuarios.First(Request => Request.Usuario1.Equals(Object.UsuarioUsuarios)).Id,
               Fecha = Object.Fecha,
               Hora = Object.Hora
            };

            _context.Asistencias.Add(Data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool AsistenciaExists(int id)
        {
            return (_context.Asistencias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
