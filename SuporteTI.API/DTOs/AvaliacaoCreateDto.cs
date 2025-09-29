using System.ComponentModel.DataAnnotations;

namespace SuporteTI.API.DTOs
{
    public class AvaliacaoCreateDto
    {
        [Required]
        public int IdChamado { get; set; }

        [Range(0, 5)]
        public int Nota { get; set; } = 0;

        public string? Comentario { get; set; }

    }
}
