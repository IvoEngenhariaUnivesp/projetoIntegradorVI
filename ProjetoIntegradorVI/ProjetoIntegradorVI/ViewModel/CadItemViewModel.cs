using ProjetoIntegradorVI.Database;
using ProjetoIntegradorVI.Domain.Model;
using ProjetoIntegradorVI.Domain.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProjetoIntegradorVI.ViewModel
{
    public class CadItemViewModel
    {
        public FirebaseConfig<EventoItem> _eventoItemFirebase;
        public Command getGramasCommand { get; set; }
        public Command ResultadoCommand { get; set; }
        public string getGramas { get; set; }
        public string getNome { get; set; }
        public TipoItemEnum getTipo { get; set; }
        private Usuario _usuarioLogado { get; set; }

        public CadItemViewModel(Usuario usuario)
        {
            _usuarioLogado = usuario;
            ResultadoCommand = new Command(CadastrarItemEvento);
        }

        public async void CadastrarItemEvento()
        {
            _eventoItemFirebase = new FirebaseConfig<EventoItem>();
            EventoItem eventoItem = new EventoItem();
            eventoItem.Nome = getNome;
            eventoItem.Tipo = getTipo;

            var cadItemEvento = await _eventoItemFirebase.InsertEventoItemAsync(eventoItem, _usuarioLogado);
            if(cadItemEvento != null)
            {
                await App.Current.MainPage.DisplayAlert("Cadastro", "Sucesso no Cadastro!", "Ok");

                App.Current.MainPage = new NavigationPage(new View.Eventos(_usuarioLogado));

                //await App.Current.MainPage.Navigation.PushModalAsync(new View.TabbedPageEventoDetalhe(_usuarioLogado, cadEventoSucces.ID.Value));
            }
            else
                await App.Current.MainPage.DisplayAlert("ERRO!", "Erro no Cadastro!", "Ok");
        }


    }
}
