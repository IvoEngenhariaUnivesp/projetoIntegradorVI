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
    public partial class Login : ContentPage
    {
        private readonly FirebaseConfig<Usuario> _client;
        public Login()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        public Login(bool facebookUser, string nome, string id, string email)
        {
            // Inicia a tela
            InitializeComponent();

            // Atribui o binding ao LoginViewModel
            BindingContext = new LoginViewModel();

            _client = new FirebaseConfig<Usuario>();

            // Se o login do facebook for bem sucedido, inicia a validação
            if (facebookUser) {

                // Busca o usuário pelo email do facebook
                Usuario userByEmail = null;
                Task.Run(async () => {
                    userByEmail = await _client.GetUsuarioByEmailAsync(email);
                }).Wait();

                // Se o usuário existir, verifica se o usuário tem a flag "IsFacebookUser" e loga
                // Se não, faz o cadastro do usuário com os dados e cadastra com base no email do facebook
                if (userByEmail != null && userByEmail.isFacebookUser)
                {
                    App.Current.MainPage = new View.Eventos(userByEmail);
                }
                else
                {
                    Usuario cadUser = null;
                    Task.Run(async () => {
                        cadUser = await _client.InsertUsuarioAsync(new Usuario { Email = email, Nome = nome, Celular = "", Senha = new Random().Next().ToString(), isFacebookUser = true });
                        App.Current.MainPage = new View.Eventos(cadUser);
                    }).Wait();
                }

            }
        }

        public string Nome { get; }
        public string Email { get; }
    }
}