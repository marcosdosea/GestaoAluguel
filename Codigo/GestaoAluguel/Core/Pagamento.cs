using System;
using System.Collections.Generic;

namespace Core;

public partial class Pagamento
{
    public int Id { get; set; }

    public float Valor { get; set; }

    public DateTime DataPagamento { get; set; }

    /// <summary>
    /// P - pix
    /// D - débito
    /// C - crédito
    /// E - espécime
    /// </summary>
    public string TipoPagamento { get; set; } = null!;

    public virtual ICollection<Cobranca> IdCobrancas { get; set; } = new List<Cobranca>();
}
