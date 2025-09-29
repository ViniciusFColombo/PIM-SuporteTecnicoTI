using System;
using System.Collections.Generic;

namespace SuporteTI.Models.Models;

public partial class Avaliacao
{
    public int IdAvaliacao { get; set; }

    public int IdChamado { get; set; }

    public int Nota { get; set; }

    public string? Comentario { get; set; }

    public virtual Chamado IdChamadoNavigation { get; set; } = null!;
    public DateTime DataAvaliacao { get; set; }
}
