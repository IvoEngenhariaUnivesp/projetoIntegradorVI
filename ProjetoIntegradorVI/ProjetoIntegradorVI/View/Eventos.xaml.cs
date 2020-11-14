using ProjetoIntegradorVI.Database;
using ProjetoIntegradorVI.Domain.Model;
using ProjetoIntegradorVI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoIntegradorVI.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Eventos : ContentPage
    {
        private Usuario _usuarioLogado;
        private List<Evento> lstEventos;
        private FirebaseConfig<Evento> _clientEvento = new FirebaseConfig<Evento>();

        public Eventos(Usuario usuarioLogado)
        {
            InitializeComponent();
            BindingContext = new EventosViewModel(usuarioLogado);

            // Recebe o usuário logado da tela de cadastro/login
            this._usuarioLogado = usuarioLogado;

            // Busca a lista de eventos do usuário
            Task.Run(async () => {
                this.lstEventos = await _clientEvento.GetAllEventosByUsuarioIDAsync(usuarioLogado.ID.Value);
            }).Wait();

            // Atribui a lista ao ListView
            lvEventos.ItemsSource = this.lstEventos;
        }

        private void BuscaEvento_SearchButtonPressed(object sender, EventArgs e)
        {
            var searchBar = sender as SearchBar;

            if(!Regex.IsMatch(searchBar.Text, "^[0-9]*$"))
                App.Current.MainPage.DisplayAlert("Erro", "A pesquisa deve ser feita com apenas com caracteres numéricos", "Ok");
            else
            {
                // Cria o client dos Eventos
                var firebaseClient = new FirebaseConfig<Evento>();
                Evento eventoResponse = new Evento();
                // Busca a lista de eventos do usuário
                Task.Run(async () => {
                    eventoResponse = await firebaseClient.GetEventoByEventoIDAsync(Int64.Parse(searchBar.Text));
                }).Wait();

                if (eventoResponse != null)
                    App.Current.MainPage.Navigation.PushModalAsync(new View.TabbedPageEventoDetalhe(_usuarioLogado, Int64.Parse(searchBar.Text)));
                else
                    App.Current.MainPage.DisplayAlert("Ah, que pena :(", "Não foi possível localizar o evento.", "Ok");
            }
        }

        private void lvEventos_Refreshing(object sender, EventArgs e)
        {
            // Busca a lista de eventos do usuário
            Task.Run(async () => {
                lvEventos.IsRefreshing = true;
                this.lstEventos = await _clientEvento.GetAllEventosByUsuarioIDAsync(_usuarioLogado.ID.Value);
                lvEventos.IsRefreshing = false;
            }).Wait();
        }
    }
}