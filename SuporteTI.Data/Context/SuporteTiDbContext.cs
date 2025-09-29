using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace SuporteTI.Data.Models;

public partial class SuporteTiDbContext : DbContext
{
    public SuporteTiDbContext()
    {
    }

    public SuporteTiDbContext(DbContextOptions<SuporteTiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Anexo> Anexos { get; set; }

    public virtual DbSet<Avaliacao> Avaliacoes { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Chamado> Chamados { get; set; }

    public virtual DbSet<Iaprocessamento> Iaprocessamentos { get; set; }

    public virtual DbSet<Interacao> Interacoes { get; set; }

    public virtual DbSet<Relatorio> Relatorios { get; set; }

    public virtual DbSet<SolucaoSugeridum> SolucaoSugerida { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-JHUP58F;Database=SuporteTI_IA;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Anexo>(entity =>
        {
            entity.HasKey(e => e.IdAnexo).HasName("PK__Anexo__F5D8C19EC40D4A6C");

            entity.ToTable("Anexo");

            entity.Property(e => e.IdAnexo).HasColumnName("id_anexo");
            entity.Property(e => e.CaminhoArquivo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("caminho_arquivo");
            entity.Property(e => e.DataEnvio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("data_envio");
            entity.Property(e => e.IdChamado).HasColumnName("id_chamado");
            entity.Property(e => e.NomeArquivo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nome_arquivo");

            entity.HasOne(d => d.IdChamadoNavigation).WithMany(p => p.Anexos)
                .HasForeignKey(d => d.IdChamado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Anexo__id_chamad__5165187F");
        });

        modelBuilder.Entity<Avaliacao>(entity =>
        {
            entity.HasKey(e => e.IdAvaliacao).HasName("PK__Avaliaca__272BC05D26FD4B32");

            entity.ToTable("Avaliacao");

            entity.Property(e => e.IdAvaliacao).HasColumnName("id_avaliacao");
            entity.Property(e => e.Comentario)
                .HasColumnType("text")
                .HasColumnName("comentario");
            entity.Property(e => e.IdChamado).HasColumnName("id_chamado");
            entity.Property(e => e.Nota).HasColumnName("nota");

            entity.HasOne(d => d.IdChamadoNavigation).WithMany(p => p.Avaliacaos)
                .HasForeignKey(d => d.IdChamado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Avaliacao__id_ch__4E88ABD4");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__CD54BC5AA0D2A4BA");

            entity.HasIndex(e => e.Nome, "UQ__Categori__6F71C0DC3CB608D1").IsUnique();

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Chamado>(entity =>
        {
            entity.HasKey(e => e.IdChamado).HasName("PK__Chamado__FB9E7C1EA950C98B");

            entity.ToTable("Chamado");

            entity.Property(e => e.IdChamado).HasColumnName("id_chamado");
            entity.Property(e => e.DataAbertura)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("data_abertura");
            entity.Property(e => e.DataFechamento)
                .HasColumnType("datetime")
                .HasColumnName("data_fechamento");
            entity.Property(e => e.Descricao)
                .HasColumnType("text")
                .HasColumnName("descricao");
            entity.Property(e => e.IdTecnico).HasColumnName("id_tecnico");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Prioridade)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("prioridade");
            entity.Property(e => e.StatusChamado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status_chamado");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdTecnicoNavigation).WithMany(p => p.ChamadoIdTecnicoNavigations)
                .HasForeignKey(d => d.IdTecnico)
                .HasConstraintName("FK_Chamado_Usuario_Tecnico");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ChamadoIdUsuarioNavigations)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Chamado__id_usua__3B75D760");

            entity.HasMany(d => d.IdCategoria).WithMany(p => p.IdChamados)
                .UsingEntity<Dictionary<string, object>>(
                    "ChamadoCategorium",
                    r => r.HasOne<Categorium>().WithMany()
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Chamado_C__id_ca__4316F928"),
                    l => l.HasOne<Chamado>().WithMany()
                        .HasForeignKey("IdChamado")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Chamado_C__id_ch__4222D4EF"),
                    j =>
                    {
                        j.HasKey("IdChamado", "IdCategoria").HasName("PK__Chamado___474B37DB8737D1C4");
                        j.ToTable("Chamado_Categoria");
                        j.IndexerProperty<int>("IdChamado").HasColumnName("id_chamado");
                        j.IndexerProperty<int>("IdCategoria").HasColumnName("id_categoria");
                    });
        });

        modelBuilder.Entity<Iaprocessamento>(entity =>
        {
            entity.HasKey(e => e.IdProcessamento).HasName("PK__IAProces__03F4709E2A3F11AA");

            entity.ToTable("IAProcessamento");

            entity.Property(e => e.IdProcessamento).HasColumnName("id_processamento");
            entity.Property(e => e.DataProcessamento)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("data_processamento");
            entity.Property(e => e.EntradaTexto)
                .HasColumnType("text")
                .HasColumnName("entrada_texto");
            entity.Property(e => e.IdChamado).HasColumnName("id_chamado");
            entity.Property(e => e.SaidaClassificacao)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("saida_classificacao");
            entity.Property(e => e.SolucaoSugerida)
                .HasColumnType("text")
                .HasColumnName("solucao_sugerida");

            entity.HasOne(d => d.IdChamadoNavigation).WithMany(p => p.Iaprocessamentos)
                .HasForeignKey(d => d.IdChamado)
                .HasConstraintName("FK__IAProcess__id_ch__619B8048");
        });

        modelBuilder.Entity<Interacao>(entity =>
        {
            entity.HasKey(e => e.IdInteracao).HasName("PK__Interaca__FC7DC95E04662A96");

            entity.ToTable("Interacao");

            entity.Property(e => e.IdInteracao).HasColumnName("id_interacao");
            entity.Property(e => e.DataHora)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("data_hora");
            entity.Property(e => e.IdChamado).HasColumnName("id_chamado");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Mensagem)
                .HasColumnType("text")
                .HasColumnName("mensagem");

            entity.HasOne(d => d.IdChamadoNavigation).WithMany(p => p.Interacaos)
                .HasForeignKey(d => d.IdChamado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Interacao__id_ch__45F365D3");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Interacaos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Interacao__id_us__46E78A0C");
        });

        modelBuilder.Entity<Relatorio>(entity =>
        {
            entity.HasKey(e => e.IdRelatorio).HasName("PK__Relatori__616D53BD1803E78B");

            entity.ToTable("Relatorio");

            entity.Property(e => e.IdRelatorio).HasColumnName("id_relatorio");
            entity.Property(e => e.Conteudo)
                .HasColumnType("text")
                .HasColumnName("conteudo");
            entity.Property(e => e.DataGeracao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("data_geracao");
            entity.Property(e => e.IdAdministrador).HasColumnName("id_administrador");
            entity.Property(e => e.Tipo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tipo");

            entity.HasOne(d => d.IdAdministradorNavigation).WithMany(p => p.Relatorios)
                .HasForeignKey(d => d.IdAdministrador)
                .HasConstraintName("FK__Relatorio__id_ad__5DCAEF64");
        });

        modelBuilder.Entity<SolucaoSugeridum>(entity =>
        {
            entity.HasKey(e => e.IdSolucao).HasName("PK__Solucao___B9157F5BA329FCE6");

            entity.ToTable("Solucao_Sugerida");

            entity.Property(e => e.IdSolucao).HasColumnName("id_solucao");
            entity.Property(e => e.Aceita)
                .HasDefaultValue(false)
                .HasColumnName("aceita");
            entity.Property(e => e.Conteudo)
                .HasColumnType("text")
                .HasColumnName("conteudo");
            entity.Property(e => e.IdChamado).HasColumnName("id_chamado");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdChamadoNavigation).WithMany(p => p.SolucaoSugerida)
                .HasForeignKey(d => d.IdChamado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Solucao_S__id_ch__4AB81AF0");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__4E3E04ADFB23410B");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Email, "UQ__Usuario__AB6E6164734C1844").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Ativo)
                .HasDefaultValue(true)
                .HasColumnName("ativo");
            entity.Property(e => e.Cpf)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("cpf");
            entity.Property(e => e.DataNascimento).HasColumnName("data_nascimento");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Endereco)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("endereco");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nome");
            entity.Property(e => e.Senha)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("senha");
            entity.Property(e => e.Telefone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("telefone");
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
