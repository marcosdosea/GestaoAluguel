using System;
using System.Collections.Generic;

namespace Core;

public partial class Pessoa
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string? Rg { get; set; }

    public string Email { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    /// <summary>
    /// S - Solteiro
    /// C - Casado
    /// D - Divorciado
    /// U - União Estável
    /// V - Viúvo
    /// 
    /// </summary>
    public string EstadoCivil { get; set; } = null!;

    public string Profissao { get; set; } = null!;

    public string Logradouro { get; set; } = null!;

    public string Uf { get; set; } = null!;

    public string Numero { get; set; } = null!;

    public string Cidade { get; set; } = null!;

    public string Bairro { get; set; } = null!;

    public DateTime Nascimento { get; set; }

    public string Cep { get; set; } = null!;

    public string? NomeConjuge { get; set; }

    /// <summary>
    /// P - Proprietário
    /// I - Inquilino
    /// </summary>
    public string TipoPessoa { get; set; } = null!;

    public virtual ICollection<ChamadoReparo> Chamadoreparos { get; set; } = new List<ChamadoReparo>();

    public virtual ICollection<Imovel> Imovels { get; set; } = new List<Imovel>();

    public virtual ICollection<Locacao> Locacaos { get; set; } = new List<Locacao>();
}
