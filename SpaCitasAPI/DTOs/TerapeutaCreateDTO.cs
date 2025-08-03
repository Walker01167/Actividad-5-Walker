using System.ComponentModel.DataAnnotations;

namespace SpaCitasAPI.DTOs
{
    public class TerapeutaCreateDTO
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        [Phone]
        public string Telefono { get; set; }
    }
}
