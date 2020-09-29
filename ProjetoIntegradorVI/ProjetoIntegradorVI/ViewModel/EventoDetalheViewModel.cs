using ProjetoIntegradorVI.Database;
using ProjetoIntegradorVI.Domain.Model;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoIntegradorVI.ViewModel
{
    public class EventoDetalheViewModel
    {
        private readonly FirebaseConfig<Evento> _firebaseClient;
        //public Command backCommand { get; set; }//btn voltar no topo
        //public Command infoCommand { get; set; }//btn de informações no topo a direita
        //public Command eventoCommand { get; set; }//btn no rodapé para o evento
        //public Command itensCommand { get; set; }//btn no rodapé para ver os itens
        //public Command conviteCommand { get; set; }//btn no rodapé para ver os convites
        public Command cadItemEventoItemCommand { get; set; }//btn de cadastrar novo item para usuario criador
        public Command cadItemEventoItemUsuarioCommand { get; set; }//btn de cadastrar novo item para usuario convidado()
        private Usuario _usuarioLogado { get; set; }
        private long _eventoID { get; set; }
        public string NomeEvento { get; set; }
        public string DescricaoEvento { get; set; }
        public string dataInicio { get; set; }
        public string dataTermino { get; set; }
        public string horaInicio { get; set; }
        public string horaTermino { get; set; }
        public string diasRestantes { get; set; }
        public string convidadosAceitos { get; set; }
        public string convidadosRecusados { get; set; }
        public string convidadosPendentes { get; set; }

        public EventoDetalheViewModel(Usuario usuarioLogado, long eventoID)
        {
            // Instancia as variaveis de acesso a tela
            var eventoDetalhe = new EventoDetalhe();
            cadItemEventoItemCommand = new Command(AddItemEventoUser);
            _usuarioLogado = usuarioLogado;
            _firebaseClient = new FirebaseConfig<Evento>();
            _eventoID = eventoID;
            // Busca o objeto de modelo para a tela
            Task.Run(async () => {
                eventoDetalhe = await _firebaseClient.GetEventoDetalheByEventoIDAsync(eventoID);
            }).Wait();

            // Monta as propriedades da tela
            NomeEvento = eventoDetalhe.NomeEvento;
            DescricaoEvento = eventoDetalhe.DescricaoEvento;
            dataInicio = eventoDetalhe.DataInicio;
            dataTermino = eventoDetalhe.DataTermino;
            horaInicio = eventoDetalhe.HoraInicio;
            horaTermino = eventoDetalhe.HoraTermino;
            diasRestantes = String.Format("Faltam {0} dias!", eventoDetalhe.DiasRestantes);
            convidadosAceitos = eventoDetalhe.ConvitesAceitos.ToString();
            convidadosRecusados = eventoDetalhe.ConvitesRecusados.ToString();
            convidadosPendentes = eventoDetalhe.ConvitesPendentes.ToString();

            //cadItemEventoItemCommand = new Command(AddItemEventoUser);
        }

        public void AddItemEventoUser()
        {
            //App.Current.MainPage.DisplayAlert("Teste", "Testes2", "OK");
            App.Current.MainPage.Navigation.PushModalAsync(new View.CadastrarItem(_usuarioLogado));
        }

    }
}
