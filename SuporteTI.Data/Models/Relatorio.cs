using System;
using System.Collections.Generic;

namespace SuporteTI.Data.Models;

public partial class Relatorio
{
    public int IdRelatorio { get; set; }

    public string Tipo { get; set; } = null!;

    public DateTime? DataGeracao { get; set; }

    public string Conteudo { get; set; } = null!;

    public int? IdAdministrador { get; set; }

    public virtual Usuario? IdAdministradorNavigation { get; set; }
}
