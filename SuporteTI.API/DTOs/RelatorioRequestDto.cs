using System;

namespace SuporteTI.API.DTOs
{
    public class RelatorioRequestDto
    {
        public string? Tipo { get; set; }
        public int? IdTecnico { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdCategoria { get; set; }
        public string? Status { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public bool IncluirDetalhes { get; set; } = false;
        public bool AgruparPorTecnico { get; set; } = false;
    }
}
