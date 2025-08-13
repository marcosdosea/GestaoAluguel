using System;
using System.Collections.Generic;

namespace Core;

public partial class Imovel
{
    public int Id { get; set; }

    public sbyte EstaAlugado { get; set; }

    public string Apelido { get; set; } = null!;

    public float ValorAluguel { get; set; }

    public string Logradouro { get; set; } = null!;

    public string Uf { get; set; } = null!;

    public string Cep { get; set; } = null!;

    public string Numero { get; set; } = null!;

    public string Cidade { get; set; } = null!;

    public string Bairro { get; set; } = null!;

    public int IdProprietario { get; set; }

    public virtual ICollection<Chamadoreparo> Chamadoreparos { get; set; } = new List<Chamadoreparo>();

    public virtual Pessoa IdProprietarioNavigation { get; set; } = null!;

    public virtual ICollection<Locacao> Locacaos { get; set; } = new List<Locacao>();
}
