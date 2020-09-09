using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ProjetoIntegradorVI.Domain.Model;
using ProjetoIntegradorVI.Database;

namespace ProjetoIntegradorVI.ViewModel
{
    public class CadUserViewModel
    {
        private readonly FirebaseConfig<Usuario> _firebase;
        public Command ResultadoCommand { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string cell { get; set; }
        public string senha { get; set; }
        public string confirmaSenha { get; set; }

        public CadUserViewModel()
        {
            _firebase = new FirebaseConfig<Usuario>();
            ResultadoCommand = new Command(CadastrarUsuario);
        }

        public async void CadastrarUsuario()
        {
            Usuario usuario = new Usuario();
            usuario.Nome = nome;
            usuario.Email = email;
            usuario.Celular = cell;
            usuario.Senha = senha;
            if(confirmaSenha != senha)
            {
                await App.Current.MainPage.DisplayAlert("Erro!", "Senha não confere!", "Ok");
            }
            else
            {
                var cadUser = await _firebase.InsertUsuarioAsync(usuario);

                if (cadUser != null)
                {
                    await App.Current.MainPage.DisplayAlert("Cadastro", "Sucesso no Cadastro!", "Ok");

                    App.Current.MainPage = new NavigationPage(new View.Eventos(cadUser));
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Cadastro", "Houve um erro no cadastro. É possível que o este e-mail já exista.", "Voltar");
                }
            }

        }

    }
}
