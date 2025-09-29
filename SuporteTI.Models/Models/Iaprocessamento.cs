using System;
using System.Collections.Generic;

namespace SuporteTI.Models.Models;

public partial class Iaprocessamento
{
    public int IdProcessamento { get; set; }

    public string EntradaTexto { get; set; } = null!;

    public string? SaidaClassificacao { get; set; }

    public string? SolucaoSugerida { get; set; }

    public DateTime? DataProcessamento { get; set; }

    public int? IdChamado { get; set; }

    public virtual Chamado? IdChamadoNavigation { get; set; }
}
