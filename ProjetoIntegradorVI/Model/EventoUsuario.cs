using ProjetoIntegradorVI.Model.Common;
using ProjetoIntegradorVI.Model.Enums;

namespace ProjetoIntegradorVI.Model
{
    public class EventoUsuario : EntidadeBase
    {
        public long EventoID { get; set; }
        public long UsuarioMembroID { get; set; }
        public StatusConviteEnum StatusConvite { get; set; }
    }
}
