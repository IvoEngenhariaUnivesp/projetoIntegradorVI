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
    public partial class EventoDetalhe : ContentPage
    {
        public EventoDetalhe(Usuario usuarioLogado)
        {
            InitializeComponent();
            BindingContext = new EventoDetalheViewModel(usuarioLogado);
        }
    }
}