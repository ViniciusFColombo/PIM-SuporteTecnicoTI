namespace SuporteTI.API.DTOs
{
    
    public class IAResponseDto
    {
        public int IdChamado { get; set; }
        public string Classificacao { get; set; } = string.Empty;
        public string SolucaoSugerida { get; set; } = string.Empty;
    }
}
