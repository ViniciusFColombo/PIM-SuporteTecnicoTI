using System;
using System.Collections.Generic;

namespace SuporteTI.Models.Models;

public partial class SolucaoSugeridum
{
    public int IdSolucao { get; set; }

    public int IdChamado { get; set; }

    public string Titulo { get; set; } = null!;

    public string Conteudo { get; set; } = null!;

    public bool? Aceita { get; set; }

    public virtual Chamado IdChamadoNavigation { get; set; } = null!;
}
