using ProjetoIntegradorVI.Domain.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoIntegradorVI.Domain.Model
{
    public class EventoConvidadoDetalhe : EntidadeBase
    {
        public string Nome { get; set; }
        public string ItemQuantidade { get; set; }
    }
}
