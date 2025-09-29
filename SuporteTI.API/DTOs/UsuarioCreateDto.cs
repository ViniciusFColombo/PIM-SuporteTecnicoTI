using System.ComponentModel.DataAnnotations;

namespace SuporteTI.API.DTOs
{
    public class UsuarioCreateDto
    {
        [Required, StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 6)]
        public string Senha { get; set; } = string.Empty; // → no backend, armazene hash

        [Required]
        public string Tipo { get; set; } = "Cliente";

        public string? Cpf { get; set; }
        public string? Telefone { get; set; }
        public string? Endereco { get; set; }
        public DateOnly DataNascimento { get; set; }
    }
}
