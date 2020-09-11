using ProjetoIntegradorVI.Domain.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoIntegradorVI.Domain.Model
{
    public class Usuario : EntidadeBase
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Senha { get; set; }
        public bool isFacebookUser { get; set; }

    }
}
