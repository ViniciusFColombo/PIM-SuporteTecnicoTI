using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuporteTI.Data.Models;

[Table("Solucao_Sugerida")]
public partial class SolucaoSugeridum
{
    public int IdSolucao { get; set; }

    public int IdChamado { get; set; }

    public string Titulo { get; set; } = null!;

    public string Conteudo { get; set; } = null!;

    public bool? Aceita { get; set; }

    public virtual Chamado IdChamadoNavigation { get; set; } = null!;

    [Column("data_criacao")]
    public DateTime DataCriacao { get; set; }
}
