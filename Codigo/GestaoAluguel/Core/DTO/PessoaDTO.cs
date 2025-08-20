﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class PessoaDTO
    {
        public int Id { get; set; }

        public string Nome { get; set; } = null!;

        public string Cpf { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Telefone { get; set; } = null!;
    }
}
