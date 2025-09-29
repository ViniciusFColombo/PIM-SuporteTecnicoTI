using System.ComponentModel.DataAnnotations;

namespace SuporteTI.API.DTOs
{
    public class ChamadoCreateDto
    {
        [Required, StringLength(100)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Descricao { get; set; } = string.Empty;

        public string Prioridade { get; set; } = "Media";

        [Required]
        public int IdUsuario { get; set; }

        // ids de categorias (opcional — IA pode preencher depois)
        public List<int>? IdCategorias { get; set; }
    }
}
