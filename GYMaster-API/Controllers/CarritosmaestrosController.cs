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
    public class CarritosmaestrosController : ControllerBase
    {
        private readonly GymasterContext _context;

        public CarritosmaestrosController(GymasterContext context)
        {
            _context = context;
        }

        [HttpGet("Buscar")]
        public async Task<ActionResult<CarritosmaestroDTO>> GetCarritosmaestro(string UsuarioUsuarios)
        {
            CarritosmaestroDTO Carrito = await _context.Carritosmaestros.Where(
                    Request => Request.IdUsuario.Equals(
                            _context.Usuarios.First(
                                    Data => Data.Usuario1.Equals(UsuarioUsuarios)
                                ).Id
                        )
                ).Select(
                    Data => new CarritosmaestroDTO
                    {
                        UsuarioUsuarios = UsuarioUsuarios,
                        Total = Data.Total,
                    }
            ).FirstAsync();

            if (Carrito == null)
            {
                return NotFound();
            }
            else
            {
                return Carrito;
            }
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult> PostCarritosmaestro(string UsuarioUsuarios)
        {
            var Data = new Carritosmaestro()
            {
                IdUsuario = _context.Usuarios.First(Request => Request.Usuario1.Equals(UsuarioUsuarios)).Id,
                Total = 0
            };

            _context.Carritosmaestros.Add(Data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("Actualizar")]
        public async Task<ActionResult> PutCarritosmaestro(CarritosmaestroDTO Object)
        {
            var Data = await _context.Carritosmaestros.FirstAsync(
                Request => Request.IdUsuario.Equals(
                        _context.Usuarios.First(
                                Data => Data.Usuario1.Equals(Object.UsuarioUsuarios)
                            ).Id
                    )
            );
            Data.Total = Object.Total;

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool CarritosmaestroExists(int id)
        {
            return (_context.Carritosmaestros?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
