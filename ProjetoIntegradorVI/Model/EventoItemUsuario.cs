using ProjetoIntegradorVI.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoIntegradorVI.Model
{
    public class EventoItemUsuario : EntidadeBase
    {
        public long EventoID { get; set; }
        public long ItemID { get; set; }
        public long UsuarioID { get; set; }
    }
}
