using System.ComponentModel.DataAnnotations;

namespace SuporteTI.API.DTOs
{
    public class AnexoCreateDto
    {
        [Required]
        public int IdChamado { get; set; }

        [Required]
        public string NomeArquivo { get; set; } = string.Empty;

        [Required]
        public string CaminhoArquivo { get; set; } = string.Empty; // caminho relativo/URL
    }
}
