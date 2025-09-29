using System;
using System.Collections.Generic;

namespace SuporteTI.Models.Models;

public partial class Categorium
{
    public int IdCategoria { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Chamado> Chamados { get; set; } = new List<Chamado>();
}
