using System;
using System.Collections.Generic;

namespace Core;

public partial class Chamadoreparo
{
    public int Id { get; set; }

    /// <summary>
    /// C - Cadastrada
    /// P - Em progresso
    /// R - Resolvido
    /// </summary>
    public string Status { get; set; } = null!;

    public string? Tipo { get; set; }

    public string? Descricao { get; set; }

    public DateTime DataCadastro { get; set; }

    public DateTime? DataResolucao { get; set; }

    public sbyte EstaResolvido { get; set; }

    public int IdImovel { get; set; }

    public int IdInquilino { get; set; }

    public virtual Imovel IdImovelNavigation { get; set; } = null!;

    public virtual Pessoa IdInquilinoNavigation { get; set; } = null!;
}
