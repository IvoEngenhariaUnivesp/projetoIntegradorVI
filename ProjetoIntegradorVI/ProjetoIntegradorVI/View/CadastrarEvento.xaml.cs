using ProjetoIntegradorVI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoIntegradorVI.Domain.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoIntegradorVI.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastrarEvento : ContentPage
    {

        public CadastrarEvento(Usuario usuarioLogado)
        {
            InitializeComponent();
            BindingContext = new CadEventViewModel(usuarioLogado);
        }
    }
}