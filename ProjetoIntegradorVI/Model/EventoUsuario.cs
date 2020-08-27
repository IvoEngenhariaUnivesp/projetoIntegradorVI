using ProjetoIntegradorVI.Model.Common;

namespace ProjetoIntegradorVI.Model
{
    public class EventoUsuario : EntidadeBase
    {
        public long EventoID { get; set; }
        public long UsuarioMembroID { get; set; }
    }
}
