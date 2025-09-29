using System;
using System.Collections.Generic;

namespace SuporteTI.Models.Models;

public partial class Chamado
{
    public int IdChamado { get; set; }

    public int IdUsuario { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descricao { get; set; } = null!;

    public string Prioridade { get; set; } = null!;

    public string StatusChamado { get; set; } = null!;

    public DateTime? DataAbertura { get; set; }

    public DateTime? DataFechamento { get; set; }

    public int? IdTecnico { get; set; }

    public virtual ICollection<Anexo> Anexos { get; set; } = new List<Anexo>();

    public virtual ICollection<Avaliacao> Avaliacaos { get; set; } = new List<Avaliacao>();

    public virtual ICollection<Iaprocessamento> Iaprocessamentos { get; set; } = new List<Iaprocessamento>();

    public virtual Usuario? IdTecnicoNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Interacao> Interacaos { get; set; } = new List<Interacao>();

    public virtual ICollection<SolucaoSugeridum> SolucaoSugerida { get; set; } = new List<SolucaoSugeridum>();

    public int IdCategoria { get; set; }   // Chave estrangeira
    public Categorium Categoria { get; set; } = null!; // Propriedade de navegação

}
