using ProjetoMRP.Paciente.Services;
using ProjetoMRP.Paciente.ViewModel;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoMRP.Paciente.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InqueritosPage : ContentPage
    {
        public string userId;
        ViewCell lastCell;
        public InqueritosPage()
        {
            InitializeComponent();
            userId = Application.Current.Properties["UserId"].ToString();
            CarregarListaInqueritos(userId);
            lstInqueritos.IsVisible = true;
            PlaceHolder.IsVisible = false;
        }

        private async void lstInqueritos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Inqueritos inquerito = lstInqueritos.SelectedItem as Inqueritos;

            var page = new InqueritoFormPage();
            PlaceHolder.Content = page.Content;
            lstInqueritos.IsVisible = false;
            PlaceHolder.IsVisible = true;

        }
        private async void CarregarListaInqueritos(string userId)
        {
            List<Inqueritos> inqueritos = await ApiService.GetInqueritosByIdUser(Convert.ToInt32(userId));

            //var qtd = inqueritos.Count();

            lstInqueritos.ItemsSource = inqueritos;
        }

        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;

            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.AntiqueWhite;
                lastCell = viewCell;
            }
        }
    }
}