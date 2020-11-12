using Xamarin.Forms;
using ProjetoIntegradorVI.Domain.Model;
using ProjetoIntegradorVI.Database;
using System.Windows.Input;
using System.Threading.Tasks;

namespace ProjetoIntegradorVI.ViewModel
{
    public class EventosViewModel
    {
        public Command cadItemEventoItemCommand2 { get; set; }
        public Command ResultCommand { get; set; }
        public Command AdicionarEvento { get; set; }
        public ICommand AcessEventoCommand { get; set; }
        private Usuario _usuarioLogado { get; set; }
        private long _evento { get; set; }

        public EventosViewModel(Usuario usuarioLogado)
        {
            //Deixa o usuário logado em memória
            _usuarioLogado = usuarioLogado;
            ResultCommand = new Command(PushBackLogin);
            AdicionarEvento = new Command(AddEvent);
            AcessEventoCommand = new Command(async (object obj) => await AccessEventoDetalheCommand(obj));
            cadItemEventoItemCommand2 = new Command(AddItemEventoUser);
        }

        // Retorna a tela de login
        public void PushBackLogin()
        {
            App.Current.MainPage.Navigation.RemovePage(new View.Login());
        }

        // Aciona a tela de cadastro do evento
        public void AddEvent()
        {
            App.Current.MainPage.Navigation.PushModalAsync(new View.CadastrarEvento(_usuarioLogado));
        }

        // Aciona a tela de detalhes do evento
        private async Task AccessEventoDetalheCommand(object sender)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new View.TabbedPageEventoDetalhe(_usuarioLogado, (long)sender));
        }

        public void AddItemEventoUser()
        {
            App.Current.MainPage.Navigation.PushModalAsync(new View.CadastrarItem(_usuarioLogado, _evento));
        }
    }
}
