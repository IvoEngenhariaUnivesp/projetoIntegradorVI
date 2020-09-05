using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ProjetoIntegradorVI.Domain.Model;
using ProjetoIntegradorVI.Database;


namespace ProjetoIntegradorVI.ViewModel
{
    public class LoginViewModel
    {
        public Command ResultCommand { get; set; }
        public Command EventosCommand { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public string labelUserError { get; set; }
        private readonly FirebaseConfig<Usuario> _clientUsuario;

        public LoginViewModel()
        {
            _clientUsuario = new FirebaseConfig<Usuario>();
            ResultCommand = new Command(CadastroUser);
            EventosCommand = new Command(EventosUser);
        }

        public void CadastroUser()
        {
            App.Current.MainPage.Navigation.PushModalAsync(new View.CadastroUsuario());
        }

        public async void EventosUser()
        {
            Usuario usuario = new Usuario
            {
                Email = email,
                Senha = senha
            };

            var usuarioResponse = await _clientUsuario.InsertAsync("Usuarios", usuario);

            if (usuarioResponse != null)
            {
                App.Current.MainPage.DisplayAlert("Mensagem", "Login Criado e Aceito", "Voltar");
                //await App.Current.MainPage.Navigation.PushModalAsync(new View.Eventos());
            }
        }
    }
}
