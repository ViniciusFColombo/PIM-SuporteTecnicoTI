namespace SuporteTI.API.DTOs
{
    public class AvaliacaoReadDto
    {
        public int IdAvaliacao { get; set; }
        public int IdChamado { get; set; }
        public int Nota { get; set; }
        public string? Comentario { get; set; }
    }
}
