using System;
using System.Collections.Generic;

namespace Core;

public partial class Cobranca
{
    public uint Id { get; set; }

    public float Valor { get; set; }

    public DateTime DataCobranca { get; set; }

    public string TipoCobranca { get; set; } = null!;

    public uint IdLocacao { get; set; }

    public virtual Locacao IdLocacaoNavigation { get; set; } = null!;

    public virtual ICollection<Pagamento> IdPagamentos { get; set; } = new List<Pagamento>();
}
