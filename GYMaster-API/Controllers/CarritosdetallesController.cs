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
    public class CarritosdetallesController : ControllerBase
    {
        private readonly GymasterContext _context;

        public CarritosdetallesController(GymasterContext context)
        {
            _context = context;
        }

        [HttpGet("Catalogo")]
        public async Task<ActionResult<List<CarritosdetalleDTO>>> GetCarritosdetalle(string UsuarioUsuarios)
        {
            var List = await _context.Carritosmaestros.Where(
                Request => Request.IdUsuario.Equals(
                        _context.Usuarios.First(
                                Data => Data.Usuario1.Equals(UsuarioUsuarios)
                            ).Id
                    )
            ).Join(
                _context.Carritosdetalles,
                Carritomaestro => Carritomaestro.Id,
                Carritodetalle => Carritodetalle.IdCarrito,
                (TableCarritosmaestro, TableCarritosdetalle) => new CarritosdetalleDTO
                {
                    UsuarioUsuarios = UsuarioUsuarios,
                    UsuarioGimnasios = _context.Gimnasios.First(
                        Data => Data.Id.Equals(_context.Productos.First(
                            Request => Request.Id.Equals(TableCarritosdetalle.IdProducto)
                        ).IdGimnasio)
                    ).Usuario,
                    NombreProductos = _context.Productos.First(
                        Data => Data.Id.Equals(TableCarritosdetalle.IdProducto)
                    ).Nombre,
                    Cantidad = TableCarritosdetalle.Cantidad,
                    Subtotal = TableCarritosdetalle.Subtotal
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

        [HttpPost("Buscar")]
        public async Task<ActionResult<List<CarritosdetalleDTO>>> PostCarritosdetalleBuscar(string UsuarioUsuarios, ProductosDTO Object)
        {
            var Data = await _context.Carritosdetalles.Where(
                Data => Data.IdCarrito.Equals(
                    _context.Carritosmaestros.First(
                        Data => Data.IdUsuario.Equals(
                            _context.Usuarios.First(Request => Request.Usuario1.Equals(UsuarioUsuarios)).Id
                        )
                    ).Id
                ) && Data.IdProducto.Equals(
                    _context.Productos.First(
                        Data => Data.Nombre.Equals(Object.Nombre) && Data.IdGimnasio.Equals(
                            _context.Gimnasios.First(Request => Request.Usuario.Equals(Object.UsuarioGimnasios)).Id
                        )
                    ).Id
                )
            ).FirstAsync();

            if (Data == null)
            {
                return NotFound();
            }
            else
            {
                return Content("{ \"Resultado\": \"En el carrito\" }", "application/json");
            }
        }

        [HttpPost("Agregar")]
        public async Task<ActionResult> PostCarritosdetalle(CarritosdetalleDTO Object)
        {
            var Data = new Carritosdetalle()
            {
                IdCarrito = _context.Carritosmaestros.First(
                    Data => Data.IdUsuario.Equals(
                        _context.Usuarios.First(Request => Request.Usuario1.Equals(Object.UsuarioUsuarios)).Id
                    )
                ).Id,
                IdProducto = _context.Productos.First(
                    Data => Data.Nombre.Equals(Object.NombreProductos) && Data.IdGimnasio.Equals(
                        _context.Gimnasios.First(Request => Request.Usuario.Equals(Object.UsuarioGimnasios)).Id
                    )
                ).Id,
                Cantidad = Object.Cantidad,
                Subtotal = Object.Subtotal
            };
            _context.Carritosdetalles.Add(Data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("Ajustar")]
        public async Task<ActionResult> PutCarritosdetalle(CarritosdetalleDTO Object)
        {
            var Data = await _context.Carritosdetalles.FirstAsync(
                Data => Data.IdCarrito.Equals(
                    _context.Carritosmaestros.First(
                        Data => Data.IdUsuario.Equals(
                            _context.Usuarios.First(
                                Data => Data.Usuario1.Equals(Object.UsuarioUsuarios)
                            ).Id
                        )
                    ).Id
                ) && Data.IdProducto.Equals(
                    _context.Productos.First(
                        Data => Data.Nombre.Equals(Object.NombreProductos) && Data.IdGimnasio.Equals(
                            _context.Gimnasios.First(Request => Request.Usuario.Equals(Object.UsuarioGimnasios)).Id
                        )
                    ).Id
                )
            );
            Data.Cantidad = Object.Cantidad;
            Data.Subtotal = Object.Subtotal;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("Eliminar")]
        public async Task<ActionResult> DeleteCarritosdetalleProducto(CarritosdetalleDTO Object)
        {
            var Data = new Carritosdetalle()
            {
                Id = _context.Carritosdetalles.First(
                    Data => Data.IdCarrito.Equals(
                        _context.Carritosmaestros.First(
                            Data => Data.IdUsuario.Equals(
                                _context.Usuarios.First(
                                    Data => Data.Usuario1.Equals(Object.UsuarioUsuarios)
                                ).Id
                            )
                        ).Id
                    ) && Data.IdProducto.Equals(
                        _context.Productos.First(
                            Product => Product.Nombre.Equals(Object.NombreProductos) && Product.IdGimnasio.Equals(
                                _context.Gimnasios.First(Request => Request.Usuario.Equals(Object.UsuarioGimnasios)).Id
                            )
                        ).Id
                    )
                ).Id
            };

            if (Data == null)
            {
                return NotFound();
            }

            _context.ChangeTracker.Clear();
            _context.Carritosdetalles.Attach(Data);
            _context.Carritosdetalles.Remove(Data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("Limpiar")]
        public async Task<ActionResult> DeleteCarritosdetalle(string UsuarioUsuarios)
        {
            var List = await _context.Carritosmaestros.Where(
                Request => Request.IdUsuario.Equals(
                        _context.Usuarios.First(
                                Data => Data.Usuario1.Equals(UsuarioUsuarios)
                            ).Id
                    )
            ).Join(
                _context.Carritosdetalles,
                Carritomaestro => Carritomaestro.Id,
                Carritodetalle => Carritodetalle.IdCarrito,
                (TableCarritosmaestro, TableCarritosdetalle) => new Carritosdetalle
                {
                    Id = TableCarritosdetalle.Id
                }
            ).ToListAsync();

            if (List == null)
            {
                return NotFound();
            }

            foreach(Carritosdetalle Element in List)
            {
                _context.ChangeTracker.Clear();
                _context.Carritosdetalles.Attach(Element);
                _context.Carritosdetalles.Remove(Element);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        private bool CarritosdetalleExists(int id)
        {
            return (_context.Carritosdetalles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
