using System;
using System.Collections.Generic;

namespace SuporteTI.Models.Models;

public partial class Interacao
{
    public int IdInteracao { get; set; }

    public int IdChamado { get; set; }

    public int IdUsuario { get; set; }

    public string Mensagem { get; set; } = null!;

    public DateTime? DataHora { get; set; }

    public virtual Chamado IdChamadoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
