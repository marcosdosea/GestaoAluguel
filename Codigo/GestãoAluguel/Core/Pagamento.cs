using System;
using System.Collections.Generic;

namespace Core;

public partial class Pagamento
{
    public uint Id { get; set; }

    public float Valor { get; set; }

    public DateTime DataPagamento { get; set; }

    public virtual ICollection<Cobranca> IdCobrancas { get; set; } = new List<Cobranca>();
}
