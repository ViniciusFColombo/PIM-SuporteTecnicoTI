using System.ComponentModel.DataAnnotations;

namespace SuporteTI.API.DTOs
{
    public class ChamadoUpdateDto
    {
        [Required]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Descricao { get; set; } = string.Empty;

        public string Prioridade { get; set; } = "Media";

        [Required]
        public string StatusChamado { get; set; } = "Aberto";

        public List<int>? IdCategorias { get; set; } // substitui categorias se enviado
    }
}
