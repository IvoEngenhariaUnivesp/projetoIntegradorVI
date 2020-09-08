using ProjetoIntegradorVI.Domain.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoIntegradorVI.Domain.Model
{
    public class EventoItemUsuario : EntidadeBase
    {
        public long EventoItemID { get; set; }
        public long EventoID { get; set; }
        public long UsuarioID { get; set; }
        public double Quantidade { get; set; }
    }
}
