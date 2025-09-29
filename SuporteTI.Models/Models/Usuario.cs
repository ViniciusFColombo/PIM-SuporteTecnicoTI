using System;
using System.Collections.Generic;

namespace SuporteTI.Models.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public bool? Ativo { get; set; }

    public virtual ICollection<Chamado> ChamadoIdTecnicoNavigations { get; set; } = new List<Chamado>();

    public virtual ICollection<Chamado> ChamadoIdUsuarioNavigations { get; set; } = new List<Chamado>();

    public virtual ICollection<Interacao> Interacaos { get; set; } = new List<Interacao>();

    public virtual ICollection<Relatorio> Relatorios { get; set; } = new List<Relatorio>();
}
