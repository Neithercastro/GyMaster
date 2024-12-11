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
    public class EjerciciosController : ControllerBase
    {
        private readonly GymasterContext _context;

        public EjerciciosController(GymasterContext context)
        {
            _context = context;
        }

        [HttpGet("Mostrar")]
        public async Task<ActionResult<IEnumerable<EjerciciosDTO>>> GetEjercicio(int IdMusculo)
        {
            if (_context.Ejercicios == null)
            {
                return NotFound();
            }
            var ejercicio = await _context.Ejercicios
                .Where(ejercicio => ejercicio.IdMusculo == IdMusculo)
                .Join(
                    _context.Musculos,
                    ejercicio => ejercicio.IdMusculo,
                    Musculo => Musculo.Idmusculos,
                    (ejercicio, Musculo) => new EjerciciosDTO
                    {
                        IdEjercicios = ejercicio.Idejercicios,
                        Ejercicio = ejercicio.Ejercicio1,
                        ZonaTrabajo = ejercicio.ZonaTrabajo,
                        Repeticiones = ejercicio.Repeticiones,
                        Series = ejercicio.Series,
                        NombreMusculo = Musculo.Nombre
                    }
                )
                .ToListAsync();

            if (ejercicio == null)
            {
                return NotFound();
            }

            return ejercicio;
        }

        private bool EjercicioExists(int id)
        {
            return (_context.Ejercicios?.Any(e => e.Idejercicios == id)).GetValueOrDefault();
        }
    }
}