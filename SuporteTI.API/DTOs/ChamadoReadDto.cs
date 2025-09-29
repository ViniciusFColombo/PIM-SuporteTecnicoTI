namespace SuporteTI.API.DTOs
{
    public class ChamadoReadDto
    {
        public int IdChamado { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Prioridade { get; set; } = string.Empty;
        public string StatusChamado { get; set; } = string.Empty;
        public DateTime DataAbertura { get; set; }
        public DateTime? DataFechamento { get; set; }
        public UsuarioReadDto? Usuario { get; set; }
        public List<CategoriaReadDto>? Categorias { get; set; }
    }
}
