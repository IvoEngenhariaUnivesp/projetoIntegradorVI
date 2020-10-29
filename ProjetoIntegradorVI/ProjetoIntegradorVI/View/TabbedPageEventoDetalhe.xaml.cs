using ProjetoIntegradorVI.Database;
using ProjetoIntegradorVI.Domain.Model;
using ProjetoIntegradorVI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoIntegradorVI.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPageEventoDetalhe : TabbedPage
    {
        private Usuario _usuarioLogado;
        private List<EventoItem> lstEventoItems;
        public Command AceitaConviteCommand;
        public Command RemoveConviteCommand;
        public TabbedPageEventoDetalhe(Usuario usuarioLogado, long eventoID)
        {
            InitializeComponent();

            // Traz o evento do eventoID informado
            Evento evento = new Evento();
            var firebaseClient = new FirebaseConfig<Evento>();
            Task.Run(async () => {
                evento = await firebaseClient.GetEventoByEventoIDAsync(eventoID);
            }).Wait();

            // Dá contexto ao ViewModel
            BindingContext = new EventoDetalheViewModel(usuarioLogado, evento.ID.Value);

            // Busca o item do evento...
            if (tabItens != null)
            {
                EventoItem eventoItem = new EventoItem();
                Task.Run(async () =>
                {
                    this.lstEventoItems = await firebaseClient.GetEventoItemAsync(eventoID);
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
    }
}