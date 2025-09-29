using System.ComponentModel.DataAnnotations;

namespace SuporteTI.API.DTOs
{
    public class UsuarioUpdateDto
    {
        [Required]
        public int IdUsuario { get; set; }

        [Required, StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [EmailAddress, StringLength(100)]
        public string? Email { get; set; }

        public string? Cpf { get; set; }
        public string? Telefone { get; set; }
        public bool? Ativo { get; set; }
    }
}
