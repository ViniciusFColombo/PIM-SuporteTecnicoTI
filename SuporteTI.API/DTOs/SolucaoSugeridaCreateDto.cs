using System.ComponentModel.DataAnnotations;

namespace SuporteTI.API.DTOs
{
    public class SolucaoSugeridaCreateDto
    {
        [Required]
        public int IdChamado { get; set; }
        [Required]
        public string Titulo { get; set; } = string.Empty;
        [Required]
        public string Conteudo { get; set; } = string.Empty;
    }
}
