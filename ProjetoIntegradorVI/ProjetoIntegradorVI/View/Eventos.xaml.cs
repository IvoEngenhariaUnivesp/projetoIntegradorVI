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
    public partial class Eventos : ContentPage
    {
        private Usuario usuarioLogado;
        private List<Evento> lstEventos;

        //public Eventos()
        //{
        //    InitializeComponent();
        //    BindingContext = new EventosViewModel();
        //}
        public Eventos(Usuario usuarioLogado)
        {
            InitializeComponent();
            BindingContext = new EventosViewModel();

            // Cria o client dos Eventos
            var firebaseClient = new FirebaseConfig<Evento>();

            // Recebe o usuário logado da tela de cadastro/login
            this.usuarioLogado = usuarioLogado;

            // Busca a lista de eventos do usuário
            Task.Run(async () => {
                this.lstEventos = await firebaseClient.GetAllEventosByUsuarioIDAsync(usuarioLogado.ID.Value);
            }).Wait();

            // Atribui a lista ao ListView
            lvEventos.ItemsSource = this.lstEventos;
        }
    }
}