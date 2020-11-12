using Plugin.ExternalMaps;
using ProjetoIntegradorVI.Domain.Model;
using ProjetoIntegradorVI.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoIntegradorVI.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventoConvidadoDetalhe : ContentPage
    {
        public EventoConvidadoDetalhe(long usuarioMembroID)
        {
            InitializeComponent();

        }
    }
}