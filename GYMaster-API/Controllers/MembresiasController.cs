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
    public class MembresiasController : ControllerBase
    {
        private readonly GymasterContext _context;

        public MembresiasController(GymasterContext context)
        {
            _context = context;
        }

        [HttpGet("Mostrar")]
        public async Task<ActionResult<List<MembresiasDTO>>> GetMembresiasGimnasio(string UsuarioGimnasios)
        {
            var List = await _context.Membresias.Where(
                Request => Request.IdGimnasio.Equals(
                    _context.Gimnasios.First(
                        Data => Data.Usuario.Equals(UsuarioGimnasios)
                    ).Id
                ) && Request.IdEstatus == 1
            ).Select(
                  Data => new MembresiasDTO
                  {
                      UsuarioGimnasios = UsuarioGimnasios,
                      Tipo = Data.Tipo,
                      Precio = Data.Precio,
                      Periodo = Data.Periodo
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
        public async Task<ActionResult<List<MembresiasDTO>>> GetMembresias(string UsuarioGimnasios)
        {
            var List = await _context.Membresias.Where(
                Request => Request.IdGimnasio.Equals(
                    _context.Gimnasios.First(
                        Data => Data.Usuario.Equals(UsuarioGimnasios)
                    ).Id
                )
            ).Select(
                  Data => new MembresiasDTO
                  {
                      UsuarioGimnasios = UsuarioGimnasios,
                      Tipo = Data.Tipo,
                      Precio = Data.Precio,
                      Periodo = Data.Periodo,
                      IdEstatus = Data.IdEstatus
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
        public async Task<ActionResult> PostMembresia(MembresiasDTO Object)
        {
            var Data = new Membresia()
            {
                IdGimnasio = _context.Gimnasios.First(Request => Request.Usuario.Equals(Object.UsuarioGimnasios)).Id,
                Tipo = Object.Tipo,
                Precio = Object.Precio,
                Periodo = Object.Periodo,
                IdEstatus = Object.IdEstatus
            };
            _context.Membresias.Add(Data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("Modificar")]
        public async Task<ActionResult> PutMembresia(MembresiasDTO Object)
        {
            var Data = await _context.Membresias.FirstAsync(
                Request => Request.IdGimnasio.Equals(
                    _context.Gimnasios.First(
                       Data => Data.Usuario.Equals(Object.UsuarioGimnasios)
                    ).Id
                ) && Request.Tipo.Equals(Object.Tipo)
            );
            Data.Tipo = Object.Tipo;
            Data.Precio = Object.Precio;
            Data.Periodo = Object.Periodo;
            Data.IdEstatus = Object.IdEstatus;

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool MembresiaExists(int id)
        {
            return (_context.Membresias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
