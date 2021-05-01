using ProjetoMRP.Paciente.Services;
using ProjetoMRP.Paciente.ViewModel;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoMRP.Paciente.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AjudaPage : ContentPage
    {
        public string userId;
        public AjudaPage()
        {
            userId = Application.Current.Properties["UserId"].ToString();
            InitializeComponent();
            
        }
    }
}