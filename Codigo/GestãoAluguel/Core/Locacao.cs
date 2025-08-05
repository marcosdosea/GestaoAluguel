using System;
using System.Collections.Generic;

namespace Core;

public partial class Locacao
{
    public uint Id { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime? DataFim { get; set; }

    public float ValorAluguel { get; set; }

    public sbyte Status { get; set; }

    public uint IdImovel { get; set; }

    public uint IdProprietario { get; set; }

    public uint IdInquilino { get; set; }

    public byte[]? Contrato { get; set; }

    public string? Motivo { get; set; }

    public virtual ICollection<Cobranca> Cobrancas { get; set; } = new List<Cobranca>();

    public virtual Imovel IdImovelNavigation { get; set; } = null!;

    public virtual Pessoa IdInquilinoNavigation { get; set; } = null!;

    public virtual Pessoa IdProprietarioNavigation { get; set; } = null!;
}
