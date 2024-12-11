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
using System.Collections;
using System.Net.NetworkInformation;

namespace GYMaster_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly GymasterContext _context;

        public ProductosController(GymasterContext context)
        {
            _context = context;
        }

        [HttpGet("Mostrar")]
        public async Task<ActionResult<List<ProductosDTO>>> GetProductosActivos(string UsuarioGimnasios)
        {
            var List = await _context.Productos.Where(
                Request => Request.IdGimnasio.Equals(
                        _context.Gimnasios.First(
                                Data => Data.Usuario.Equals(UsuarioGimnasios)
                            ).Id
                    ) && Request.IdEstatus == 1
            ).Select(
                Data => new ProductosDTO
                {
                    UsuarioGimnasios = UsuarioGimnasios,
                    Nombre = Data.Nombre,
                    Descripcion = Data.Descripcion,
                    Precio = Data.Precio,
                    Stock = Data.Stock,
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

        [HttpGet("Catalogo")]
        public async Task<ActionResult<List<ProductosDTO>>> GetProductos(string UsuarioGimnasios)
        {
            var List = await _context.Productos.Where(
                Request => Request.IdGimnasio.Equals(
                        _context.Gimnasios.First(
                                Data => Data.Usuario.Equals(UsuarioGimnasios)
                            ).Id
                    )
            ).Select(
                Data => new ProductosDTO
                {
                    UsuarioGimnasios = UsuarioGimnasios,
                    Nombre = Data.Nombre,
                    Descripcion = Data.Descripcion,
                    Precio = Data.Precio,
                    Stock = Data.Stock,
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
        public async Task<ActionResult> PostProducto(ProductosDTO Object)
        {
            var Data = new Producto()
            {
                IdGimnasio = _context.Gimnasios.First(Request => Request.Usuario.Equals(Object.UsuarioGimnasios)).Id,
                Nombre = Object.Nombre,
                Descripcion = Object.Descripcion,
                Precio = Object.Precio,
                Stock = Object.Stock,
                IdEstatus = Object.IdEstatus
            };
            _context.Productos.Add(Data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("Modificar")]
        public async Task<ActionResult> PutProducto(string NombreProductos, ProductosDTO Object)
        {
            var Data = await _context.Productos.FirstAsync(
                Request => Request.IdGimnasio.Equals(
                    _context.Gimnasios.First(
                        Data => Data.Usuario.Equals(Object.UsuarioGimnasios)
                    ).Id
                ) && Request.Nombre.Equals(NombreProductos)
            );
            Data.IdGimnasio = Data.IdGimnasio;
            Data.Nombre = Object.Nombre;
            Data.Descripcion = Object.Descripcion;
            Data.Precio = Object.Precio;
            Data.Stock = Object.Stock;
            Data.IdEstatus = Object.IdEstatus;

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ProductoExists(int id)
        {
            return (_context.Productos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
