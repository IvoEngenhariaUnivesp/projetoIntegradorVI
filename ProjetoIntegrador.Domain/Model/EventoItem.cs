using ProjetoIntegradorVI.Domain.Model.Common;
using ProjetoIntegradorVI.Domain.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoIntegradorVI.Domain.Model
{
    public class EventoItem : EntidadeBase
    {
        public long EventoID { get; set; }
        public TipoItemEnum Tipo { get; set; }
        public string Nome { get; set; }
        public string TipoUnidade { get; set; }
    }
}
