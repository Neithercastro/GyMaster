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
    public class VentasController : ControllerBase
    {
        private readonly GymasterContext _context;

        public VentasController(GymasterContext context)
        {
            _context = context;
        }

        [HttpGet("Catalogo")]
        public async Task<ActionResult<List<VentasDTO>>> GetVenta(string UsuarioGimnasios)
        {
            var List = await _context.Ventas.Where(
                Request => Request.IdGimnasio.Equals(
                    _context.Gimnasios.First(
                        Data => Data.Usuario.Equals(UsuarioGimnasios)    
                    ).Id
                )
            ).Select(
                Data => new VentasDTO
                {
                    UsuarioGimnasios = UsuarioGimnasios,
                    Concepto = _context.Conceptos.First(Request => Request.Id.Equals(Data.IdConcepto)).Concepto1,
                    Total = Data.Total,
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

        [HttpPost("Registrar")]
        public async Task<ActionResult> PostVenta(VentasDTO Object)
        {
            var Data = new Venta()
            {
                IdGimnasio = _context.Gimnasios.First(Request => Request.Usuario.Equals(Object.UsuarioGimnasios)).Id,
                IdConcepto = _context.Conceptos.First(Request => Request.Concepto1.Equals(Object.Concepto)).Id,
                Total = Object.Total,
                Fecha = Object.Fecha,
                Hora = Object.Hora
            };

            _context.Ventas.Add(Data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool VentaExists(int id)
        {
            return (_context.Ventas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
