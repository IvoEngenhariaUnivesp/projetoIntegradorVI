using ProjetoIntegradorVI.Database;
using ProjetoIntegradorVI.Domain.Model;
using ProjetoIntegradorVI.Domain.Model.Enums;
using ProjetoIntegradorVI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoIntegradorVI.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPageEventoDetalhe : TabbedPage
    {
        #region Properties

        #region Clients Firebase
        private FirebaseConfig<Evento> _clientEvento = new FirebaseConfig<Evento>();
        private FirebaseConfig<EventoItem> _clientEventoItem = new FirebaseConfig<EventoItem>();
        private FirebaseConfig<EventoUsuario> _clientEventoUsuario = new FirebaseConfig<EventoUsuario>();
        #endregion

        #region In Memory
        private Usuario _usuarioLogado;
        private List<EventoItem> lstEventoItems;
        public long EventoID { get; set; }
        #endregion

        #endregion

        public TabbedPageEventoDetalhe(Usuario usuarioLogado, long eventoID)
        {
            // Inicializa a tela
            InitializeComponent();

            // Guarda o usuário logado e o ID do Evento
            _usuarioLogado = usuarioLogado;
            EventoID = eventoID;

            // Verifica se o usuário é membro do evento
            // Se não for, desabilita todas as abas 
            // e deixa disponível a aba pra enviar o convite
            if (IsMembroAceito(_clientEventoUsuario))
                this.Children.Remove(tabDetalhesConvitePendente);
            else
            {
                this.Children.Remove(tabItens);
                this.Children.Remove(tabDetalhes);
            }

            // Desabilita o frame de envio de convite para um usuário que esteja com seu convite pendente
            if (IsMembroPendente(_clientEventoUsuario)) btnEnviaConvite.IsVisible = false;

            // Traz o evento do eventoID informado
            Evento evento = BuscaEvento(eventoID, _clientEvento);

            // Busca e popula a lista de convites pendentes
            BuscaConvitesPendentes(eventoID, _clientEventoUsuario);
            
            // Dá contexto ao ViewModel e aos commands
            BindingContext = new EventoDetalheViewModel(usuarioLogado, evento.ID.Value);

            // Busca o item do evento...
            if (tabItens != null)
            {
                EventoItem eventoItem = new EventoItem();
                Task.Run(async () =>
                {
                    this.lstEventoItems = await _clientEventoItem.GetEventoItemAsync(eventoID);
                }).Wait();
                ;
                //lvItemEvento.ItemsSource = this.lstEventoItems;
            }

            // Remove a aba de convites caso o usuário logado não seja o criador do evento
            if (evento != null && (usuarioLogado.ID != evento.UsuarioCriadorID))
            {
                this.Children.Remove(tabConvites);
            }
        }

        public Evento BuscaEvento(long eventoID, FirebaseConfig<Evento> firebaseClient)
        {
            Evento eventoReturn = new Evento();

            Task.Run(async () => {
                eventoReturn = await firebaseClient.GetEventoByEventoIDAsync(eventoID);
            }).Wait();

            return eventoReturn;
        }

        public bool IsMembroAceito(FirebaseConfig<EventoUsuario> firebaseClient)
        {
            var isMembro = true;

            Task.Run(async () => {
                isMembro = await firebaseClient.IsUsuarioMembroAceitoByEventoIDAsync(_usuarioLogado.ID.Value, EventoID);
            }).Wait();

            return isMembro;
        }

        public bool IsMembroPendente(FirebaseConfig<EventoUsuario> firebaseClient)
        {
            var isMembro = true;

            Task.Run(async () => {
                isMembro = await firebaseClient.IsUsuarioMembroPendenteRecusadoByEventoIDAsync(_usuarioLogado.ID.Value, EventoID);
            }).Wait();

            return isMembro;
        }

        public void BuscaConvitesPendentes(long eventoID, FirebaseConfig<EventoUsuario> firebaseClient)
        {
            Task.Run(async () =>
            {
                lstConvitesPendentes.ItemsSource = await firebaseClient.GetConvitesPendentesByEventoIDAsync(eventoID);
            }).Wait();
        }

        private void AceitaConvite_Clicked(object sender, EventArgs e)
        {
            // Define a ação que será feita após o código
            bool podeRemover = true;

            // Converte o sender para ImageButton
            var parametro = sender as ImageButton;

            // Pega o CommandParameter e recebe como cast EventoUsuarioDetalhe
            EventoUsuarioDetalhe eventoUsuarioAceito = (EventoUsuarioDetalhe)parametro.CommandParameter;

            // Muda o status do convite
            eventoUsuarioAceito.StatusConvite = StatusConviteEnum.Aceito;

            // Aplica as mudanças no banco
            Task.Run(async () =>
            {
                podeRemover = await _clientEventoUsuario.SetStatusConvite(new EventoUsuario
                {
                    ID = eventoUsuarioAceito.ID,
                    EventoID = EventoID,
                    StatusConvite = eventoUsuarioAceito.StatusConvite,
                    UsuarioMembroID = eventoUsuarioAceito.UsuarioMembroID
                });
            }).Wait();

            if (podeRemover)
            {
                lstConvitesPendentes.ItemsSource = ((List<EventoUsuarioDetalhe>)lstConvitesPendentes.ItemsSource).Where(x => x.ID != eventoUsuarioAceito.ID).ToList();

                var pendentes = Int64.Parse(convidadosPendentes.Text);
                convidadosPendentes.Text = (pendentes - 1).ToString();

                var aceitos = Int64.Parse(convidadosAceitos.Text);
                convidadosAceitos.Text = (aceitos + 1).ToString();
            }
            else
                App.Current.MainPage.DisplayAlert("Erro", "Não foi possível alterar o status do convite", "Ok");
        }

        private void RecusaConvite_Clicked(object sender, EventArgs e)
        {
            // Define a ação que será feita após o código
            bool podeRemover = true;

            // Converte o sender para ImageButton
            var parametro = sender as ImageButton;

            // Pega o CommandParameter e recebe como cast EventoUsuarioDetalhe
            EventoUsuarioDetalhe eventoUsuarioRecusado = (EventoUsuarioDetalhe)parametro.CommandParameter;

            // Muda o status do convite
            eventoUsuarioRecusado.StatusConvite = StatusConviteEnum.Recusado;

            // Aplica as mudanças no banco
            Task.Run(async () =>
            {
                podeRemover = await _clientEventoUsuario.SetStatusConvite(new EventoUsuario
                {
                    ID = eventoUsuarioRecusado.ID,
                    EventoID = EventoID,
                    StatusConvite = eventoUsuarioRecusado.StatusConvite,
                    UsuarioMembroID = eventoUsuarioRecusado.UsuarioMembroID
                });
            }).Wait();

            if (podeRemover)
            {
                lstConvitesPendentes.ItemsSource = ((List<EventoUsuarioDetalhe>)lstConvitesPendentes.ItemsSource).Where(x => x.ID != eventoUsuarioRecusado.ID).ToList();

                var pendentes = Int64.Parse(convidadosPendentes.Text);
                convidadosPendentes.Text = (pendentes - 1).ToString();

                var recusados = Int64.Parse(convidadosRecusados.Text);
                convidadosRecusados.Text = (recusados + 1).ToString();
            }
            else
                App.Current.MainPage.DisplayAlert("Erro", "Não foi possível alterar o status do convite", "Ok");

        }

        private void EnviaConvite_Clicked(object sender, EventArgs e)
        {
            // Converte o sender para Button
            var parametro = sender as Button;

            var conviteEnviar = new EventoUsuario { EventoID = EventoID, StatusConvite = StatusConviteEnum.Pendente, UsuarioMembroID = _usuarioLogado.ID.Value };

            Task.Run(async () =>
            {
                await _clientEventoUsuario.InsertEventoUsuarioAsync(conviteEnviar);
            }).Wait();

            App.Current.MainPage.Navigation.PopModalAsync();
        }

        private void VisualizaLocalizacao_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushModalAsync(new View.EventoLocalizacao());
        }
    }
}