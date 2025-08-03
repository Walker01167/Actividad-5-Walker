using System.ComponentModel.DataAnnotations;

namespace SpaCitasAPI.DTOs
{
    public class ServicioCreateDTO
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Range(1, 480, ErrorMessage = "La duración debe estar entre 1 y 480 minutos.")]
        public int DuracionEnMinutos { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a cero.")]
        public decimal Precio { get; set; }
    }
}
