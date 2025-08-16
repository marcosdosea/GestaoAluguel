using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Core;

public partial class GestaoAluguelContext : DbContext
{
    public GestaoAluguelContext()
    {
    }

    public GestaoAluguelContext(DbContextOptions<GestaoAluguelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chamadoreparo> Chamadoreparos { get; set; }

    public virtual DbSet<Cobranca> Cobrancas { get; set; }

    public virtual DbSet<Imovel> Imovels { get; set; }

    public virtual DbSet<Locacao> Locacaos { get; set; }

    public virtual DbSet<Pagamento> Pagamentos { get; set; }

    public virtual DbSet<Pessoa> Pessoas { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chamadoreparo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("chamadoreparo");

            entity.HasIndex(e => e.IdImovel, "fk_ChamadoReparo_Imovel1_idx");

            entity.HasIndex(e => e.IdInquilino, "fk_ChamadoReparo_Pessoa1_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.DataCadastro)
                .HasColumnType("date")
                .HasColumnName("dataCadastro");
            entity.Property(e => e.DataResolucao)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("date")
                .HasColumnName("dataResolucao");
            entity.Property(e => e.Descricao)
                .HasMaxLength(100)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("descricao");
            entity.Property(e => e.EstaResolvido)
                .HasColumnType("tinyint(4)")
                .HasColumnName("estaResolvido");
            entity.Property(e => e.IdImovel)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idImovel");
            entity.Property(e => e.IdInquilino)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idInquilino");
            entity.Property(e => e.Status)
                .HasComment("C - Cadastrada\nP - Em progresso\nR - Resolvido")
                .HasColumnType("enum('C','P','R')")
                .HasColumnName("status");
            entity.Property(e => e.Tipo)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("enum('Encanação','Elétrica','Estrutural','Automação')")
                .HasColumnName("tipo");

            entity.HasOne(d => d.IdImovelNavigation).WithMany(p => p.Chamadoreparos)
                .HasForeignKey(d => d.IdImovel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ChamadoReparo_Imovel1");

            entity.HasOne(d => d.IdInquilinoNavigation).WithMany(p => p.Chamadoreparos)
                .HasForeignKey(d => d.IdInquilino)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ChamadoReparo_Pessoa1");
        });

        modelBuilder.Entity<Cobranca>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cobranca");

            entity.HasIndex(e => e.IdLocacao, "fk_Cobranca_Locacao1_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.DataCobranca)
                .HasColumnType("date")
                .HasColumnName("dataCobranca");
            entity.Property(e => e.Descricao)
                .HasMaxLength(45)
                .HasColumnName("descricao");
            entity.Property(e => e.IdLocacao)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idLocacao");
            entity.Property(e => e.TipoCobranca)
                .HasColumnType("enum('Aluguel','Danos','Taxas','Contas')")
                .HasColumnName("tipoCobranca");
            entity.Property(e => e.Valor).HasColumnName("valor");

            entity.HasOne(d => d.IdLocacaoNavigation).WithMany(p => p.Cobrancas)
                .HasForeignKey(d => d.IdLocacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Cobranca_Locacao1");
        });

        modelBuilder.Entity<Imovel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("imovel");

            entity.HasIndex(e => e.IdProprietario, "fk_Imovel_Pessoa1_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Apelido)
                .HasMaxLength(45)
                .HasColumnName("apelido");
            entity.Property(e => e.Bairro)
                .HasMaxLength(45)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(45)
                .HasColumnName("cidade");
            entity.Property(e => e.EstaAlugado)
                .HasColumnType("tinyint(4)")
                .HasColumnName("estaAlugado");
            entity.Property(e => e.IdProprietario)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idProprietario");
            entity.Property(e => e.Logradouro)
                .HasMaxLength(45)
                .HasColumnName("logradouro");
            entity.Property(e => e.Numero)
                .HasMaxLength(6)
                .HasColumnName("numero");
            entity.Property(e => e.Uf)
                .HasColumnType("enum('AC','AL','AP','AM','BA','CE','DF','ES','GO','MA','MT','MS','MG','PA','PB','PR','PE','PI','RJ','RN','RS','RO','RR','SC','SP','SE','TO')")
                .HasColumnName("uf");

            entity.HasOne(d => d.IdProprietarioNavigation).WithMany(p => p.Imovels)
                .HasForeignKey(d => d.IdProprietario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Imovel_Pessoa1");
        });

        modelBuilder.Entity<Locacao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("locacao");

            entity.HasIndex(e => e.IdImovel, "fk_Locacao_Imovel1_idx");

            entity.HasIndex(e => e.IdInquilino, "fk_Locacao_Pessoa2_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Contrato)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("blob")
                .HasColumnName("contrato");
            entity.Property(e => e.DataFim)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("date")
                .HasColumnName("dataFim");
            entity.Property(e => e.DataInicio)
                .HasColumnType("date")
                .HasColumnName("dataInicio");
            entity.Property(e => e.IdImovel)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idImovel");
            entity.Property(e => e.IdInquilino)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idInquilino");
            entity.Property(e => e.Motivo)
                .HasMaxLength(200)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("motivo");
            entity.Property(e => e.Status)
                .HasColumnType("tinyint(4)")
                .HasColumnName("status");
            entity.Property(e => e.ValorAluguel).HasColumnName("valorAluguel");

            entity.HasOne(d => d.IdImovelNavigation).WithMany(p => p.Locacaos)
                .HasForeignKey(d => d.IdImovel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Locacao_Imovel1");

            entity.HasOne(d => d.IdInquilinoNavigation).WithMany(p => p.Locacaos)
                .HasForeignKey(d => d.IdInquilino)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Locacao_Pessoa2");
        });

        modelBuilder.Entity<Pagamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pagamento");

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.DataPagamento)
                .HasColumnType("date")
                .HasColumnName("dataPagamento");
            entity.Property(e => e.TipoPagamento)
                .HasComment("P - pix\nD - débito\nC - crédito\nE - espécime")
                .HasColumnType("enum('C','D','P','E')")
                .HasColumnName("tipoPagamento");
            entity.Property(e => e.Valor).HasColumnName("valor");

            entity.HasMany(d => d.IdCobrancas).WithMany(p => p.IdPagamentos)
                .UsingEntity<Dictionary<string, object>>(
                    "PagamentoCobranca",
                    r => r.HasOne<Cobranca>().WithMany()
                        .HasForeignKey("IdCobranca")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_Pagamento_has_Cobranca_Cobranca1"),
                    l => l.HasOne<Pagamento>().WithMany()
                        .HasForeignKey("IdPagamento")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_Pagamento_has_Cobranca_Pagamento1"),
                    j =>
                    {
                        j.HasKey("IdPagamento", "IdCobranca").HasName("PRIMARY");
                        j.ToTable("pagamento_cobranca");
                        j.HasIndex(new[] { "IdCobranca" }, "fk_Pagamento_has_Cobranca_Cobranca1_idx");
                        j.HasIndex(new[] { "IdPagamento" }, "fk_Pagamento_has_Cobranca_Pagamento1_idx");
                        j.IndexerProperty<int>("IdPagamento")
                            .HasColumnType("int(10) unsigned")
                            .HasColumnName("idPagamento");
                        j.IndexerProperty<int>("IdCobranca")
                            .HasColumnType("int(10) unsigned")
                            .HasColumnName("idCobranca");
                    });
        });

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pessoa");

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(45)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(45)
                .HasColumnName("cidade");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("cpf");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.EstadoCivil)
                .HasComment("S - Solteiro\nC - Casado\nD - Divorciado\nU - União Estável\nV - Viúvo\n")
                .HasColumnType("enum('S','C','D','U','V')")
                .HasColumnName("estadoCivil");
            entity.Property(e => e.Logradouro)
                .HasMaxLength(45)
                .HasColumnName("logradouro");
            entity.Property(e => e.Nascimento)
                .HasColumnType("date")
                .HasColumnName("nascimento");
            entity.Property(e => e.Nome)
                .HasMaxLength(45)
                .HasColumnName("nome");
            entity.Property(e => e.NomeConjuge)
                .HasMaxLength(45)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("nomeConjuge");
            entity.Property(e => e.Numero)
                .HasMaxLength(6)
                .HasColumnName("numero");
            entity.Property(e => e.Profissao)
                .HasMaxLength(45)
                .HasColumnName("profissao");
            entity.Property(e => e.Rg)
                .HasMaxLength(12)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("rg");
            entity.Property(e => e.Telefone)
                .HasMaxLength(9)
                .IsFixedLength()
                .HasColumnName("telefone");
            entity.Property(e => e.Uf)
                .HasColumnType("enum('AC','AL','AP','AM','BA','CE','DF','ES','GO','MA','MT','MS','MG','PA','PB','PR','PE','PI','RJ','RN','RS','RO','RR','SC','SP','SE','TO')")
                .HasColumnName("uf");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
