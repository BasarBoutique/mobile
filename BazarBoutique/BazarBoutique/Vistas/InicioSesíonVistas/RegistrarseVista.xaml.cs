﻿using BazarBoutique.VistaModelos.InicioSesionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BazarBoutique.Vistas.InicioSesíonVistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrarseVista : ContentPage
    {
        public RegistrarseVista()
        {
            InitializeComponent();
            BindingContext = new RegisterViewModel(Navigation, this);
        }
    }
}