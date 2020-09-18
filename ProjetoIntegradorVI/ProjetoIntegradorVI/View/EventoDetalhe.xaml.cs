using ProjetoIntegradorVI.Domain.Model;
using ProjetoIntegradorVI.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoIntegradorVI.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventoDetalhe : ContentPage
    {
        public EventoDetalhe(Usuario usuarioLogado, long eventoID)
        {
            InitializeComponent();
            BindingContext = new EventoDetalheViewModel(usuarioLogado, eventoID);
        }
    }
}