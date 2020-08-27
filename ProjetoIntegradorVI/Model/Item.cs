using ProjetoIntegradorVI.Model.Common;
using ProjetoIntegradorVI.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoIntegradorVI.Model
{
    public class Item : EntidadeBase
    {
        public TipoItemEnum Tipo { get; set; }
        public string Nome { get; set; }
    }
}
