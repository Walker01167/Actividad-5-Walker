using System.ComponentModel.DataAnnotations;

namespace SpaCitasAPI.DTOs
{
    public class CitaCreateDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "PacienteId debe ser mayor que 0.")]
        public int PacienteId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan Hora { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ServicioId debe ser mayor que 0.")]
        public int ServicioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "TerapeutaId debe ser mayor que 0.")]
        public int TerapeutaId { get; set; }
    }
}
