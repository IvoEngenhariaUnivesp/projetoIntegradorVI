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
        public Login()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }
        public Login(bool facebookUser)
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
            V = v;
            FacebookUser = facebookUser;
        }

        public Login(bool facebookUser, string nome, string id, string email)
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
            FacebookUser = facebookUser;
            if (facebookUser) {
                Nome = nome;
                Email = email;
                user.Text = Email;
                FacebookId = id;
            }
        }

        public bool FacebookUser { get; }
        public string Nome { get; }
        public string Email { get; }
        public string FacebookId { get; }
    }
}