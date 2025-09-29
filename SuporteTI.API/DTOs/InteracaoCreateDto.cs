using System.ComponentModel.DataAnnotations;

namespace SuporteTI.API.DTOs
{
    public class InteracaoCreateDto
    {
        [Required]
        public int IdChamado { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public string Mensagem { get; set; } = string.Empty;
    }
}
