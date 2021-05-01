using ProjetoMRP.Paciente.Services;
using ProjetoMRP.Paciente.ViewModel;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoMRP.Paciente.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MensagemPage : ContentPage
    {
        public string userId;
        ViewCell lastCell;
        public MensagemPage()
        {
            
            InitializeComponent();
            
            userId = Application.Current.Properties["UserId"].ToString();
            CarregarListaMensagens(userId);
            lstMensagens.IsVisible = true;
            PlaceHolder.IsVisible = false;
        }
        private async void CarregarListaMensagens(string userId)
        {
            List<Mensagens> mensagens = await ApiService.GetMensagensByIdUser(Convert.ToInt32(userId));

            lstMensagens.ItemsSource = mensagens;
        }

        private void expResposta_Tapped(object sender, EventArgs e)
        {

        }

        private async void lstMensagens_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            
            Mensagens mensagem = lstMensagens.SelectedItem as Mensagens;

            var page = new RespostaPage(mensagem);
            PlaceHolder.Content = page.Content;
            lstMensagens.IsVisible = false;
            PlaceHolder.IsVisible = true;

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