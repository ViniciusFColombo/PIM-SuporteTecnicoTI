using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuporteTI.Data.Models;
[Table("Categoria")]
public partial class Categorium
{
    public int IdCategoria { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Chamado> IdChamados { get; set; } = new List<Chamado>();
}
