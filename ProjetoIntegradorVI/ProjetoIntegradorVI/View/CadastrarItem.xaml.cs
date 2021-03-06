﻿using ProjetoIntegradorVI.Domain.Model;
using ProjetoIntegradorVI.Domain.Model.Enums;
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
    public partial class CadastrarItem : ContentPage
    {
        CadItemViewModel cadItem = null;
        public CadastrarItem(Usuario usuario, long eventoId)
        {
            InitializeComponent();
            BindingContext = new CadItemViewModel(usuario, eventoId);
            cadItem = new CadItemViewModel(usuario, eventoId);

            var tipoEnumNames = Enum.GetNames(typeof(TipoItemEnum));

            picker.ItemsSource = tipoEnumNames;
        }
    }
}