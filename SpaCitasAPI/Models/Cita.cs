using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SpaCitasAPI.Models
{
    public class Cita
    {
        public int CitaId { get; set; }

        [Required]
        public int PacienteId { get; set; }

        [Required]
        public int ServicioId { get; set; }

        [Required]
        public int TerapeutaId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan Hora { get; set; }

        // Calculados:
        [NotMapped]
        public int Duracion => Servicio?.DuracionEnMinutos ?? 0;

        [NotMapped]
        public string Estado
        {
            get
            {
                var citaCompleta = Fecha.Date + Hora;
                var ahora = DateTime.Now;

                if (citaCompleta > ahora)
                    return "Vigente";
                else if (citaCompleta <= ahora && citaCompleta.AddMinutes(Duracion) > ahora)
                    return "En proceso";
                else
                    return "Finalizado";
            }
        }

        [NotMapped]
        public string TiempoRestante
        {
            get
            {
                var citaCompleta = Fecha.Date + Hora;
                var diferencia = citaCompleta - DateTime.Now;

                if (diferencia.TotalSeconds < 0)
                    return "Tiempo expirado";

                return $"{diferencia.Days} días, {diferencia.Hours} horas, {diferencia.Minutes} minutos";
            }
        }

        
        public Paciente Paciente { get; set; }
        public Servicio Servicio { get; set; }
        public Terapeuta Terapeuta { get; set; }
    }
}
