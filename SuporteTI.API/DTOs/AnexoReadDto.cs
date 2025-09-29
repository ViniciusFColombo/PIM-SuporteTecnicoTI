namespace SuporteTI.API.DTOs
{
    public class AnexoReadDto
    {
        public int IdAnexo { get; set; }
        public string NomeArquivo { get; set; } = string.Empty;
        public string CaminhoArquivo { get; set; } = string.Empty;
        public DateTime? DataEnvio { get; set; }
    }
}
