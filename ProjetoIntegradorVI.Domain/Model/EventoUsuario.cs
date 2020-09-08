using ProjetoIntegradorVI.Domain.Model.Common;
using ProjetoIntegradorVI.Domain.Model.Enums;

namespace ProjetoIntegradorVI.Domain.Model
{
    public class EventoUsuario : EntidadeBase
    {
        public long EventoID { get; set; }
        public long UsuarioMembroID { get; set; }
        public StatusConviteEnum StatusConvite { get; set; }
    }
}
