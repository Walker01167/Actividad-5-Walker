using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaCitasAPI.Data;
using SpaCitasAPI.Models;
using SpaCitasAPI.DTOs;
using Microsoft.AspNetCore.Authorization;


namespace SpaCitasAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Citas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CitaDetalleDTO>>> GetCitas()
        {
            var citas = await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Servicio)
                .Include(c => c.Terapeuta)
                .ToListAsync();

            var citaDTOs = citas.Select(c => new CitaDetalleDTO
            {
                Id = c.CitaId,
                PacienteNombre = c.Paciente?.Nombre,
                ServicioNombre = c.Servicio?.Nombre,
                TerapeutaNombre = c.Terapeuta?.Nombre,
                Fecha = c.Fecha,
                Hora = c.Hora,
                DuracionMinutos = c.Servicio?.DuracionEnMinutos ?? 0,
                Estado = c.Estado,
                TiempoRestante = c.TiempoRestante
            });

            return Ok(citaDTOs);

        }

        // GET: api/Citas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cita>> GetCita(int id)
        {
            var cita = await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Servicio)
                .Include(c => c.Terapeuta)
                .FirstOrDefaultAsync(c => c.CitaId == id);

            if (cita == null)
            {
                return NotFound();
            }

            return Ok(cita);
        }

        // PUT: api/Citas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCita(int id, Cita cita)
        {
            if (id != cita.CitaId)
            {
                return BadRequest();
            }

            _context.Entry(cita).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CitaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Citas
        [HttpPost]
        public async Task<ActionResult<Cita>> PostCita([FromBody] CitaCreateDTO citaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cita = new Cita
            {
                PacienteId = citaDto.PacienteId,
                Fecha = citaDto.Fecha,
                Hora = citaDto.Hora,
                ServicioId = citaDto.ServicioId,
                TerapeutaId = citaDto.TerapeutaId
            };

            _context.Citas.Add(cita);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = "Error guardando la cita: " + ex.Message });
            }

            // 🔁 Aquí se recarga la cita con sus relaciones
            await _context.Entry(cita).Reference(c => c.Paciente).LoadAsync();
            await _context.Entry(cita).Reference(c => c.Servicio).LoadAsync();
            await _context.Entry(cita).Reference(c => c.Terapeuta).LoadAsync();

            return CreatedAtAction(nameof(GetCita), new { id = cita.CitaId }, cita);
        }

        // DELETE: api/Citas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CitaExists(int id)
        {
            return _context.Citas.Any(e => e.CitaId == id);
        }

        // 🔽 🔽 🔽 Este es el nuevo endpoint personalizado
        [HttpGet("detalle")]
        public async Task<ActionResult<IEnumerable<object>>> GetCitasDetalladas()
        {
            var citas = await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Servicio)
                .Include(c => c.Terapeuta)
                .ToListAsync();

            var resultado = citas.Select(c => new
            {
                Id = c.CitaId,
                PacienteNombre = c.Paciente?.Nombre,
                ServicioNombre = c.Servicio?.Nombre,
                TerapeutaNombre = c.Terapeuta?.Nombre,
                Fecha = c.Fecha,
                Hora = c.Hora,
                DuracionMinutos = c.Duracion,
                Estado = c.Estado,
                TiempoRestante = c.TiempoRestante
            });

            return Ok(resultado);
        }


        private string ObtenerEstado(DateTime fecha, TimeSpan hora, int duracion)
        {
            var inicio = fecha.Date + hora;
            var fin = inicio.AddMinutes(duracion);
            var ahora = DateTime.Now;

            if (ahora < inicio)
                return "Vigente";
            else if (ahora >= inicio && ahora <= fin)
                return "En proceso";
            else
                return "Finalizado";
        }

        private string CalcularTiempoRestante(DateTime fecha, TimeSpan hora)
        {
            var inicio = fecha.Date + hora;
            var diferencia = inicio - DateTime.Now;

            if (diferencia.TotalSeconds <= 0)
                return "La cita ya inició o finalizó";

            return $"{(int)diferencia.TotalDays} días y {diferencia.Hours} horas";
        }
    } 
}