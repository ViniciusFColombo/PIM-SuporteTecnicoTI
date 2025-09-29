using System.ComponentModel.DataAnnotations;

namespace SuporteTI.API.DTOs
{
    public class CategoriaCreateDto
    {
        [Required, StringLength(50)]
        public string Nome { get; set; } = string.Empty;
    }
}
