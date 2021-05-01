using ProjetoMRP.Paciente.Services;
using ProjetoMRP.Paciente.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoMRP.Paciente.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RespostaPage : ContentPage
    {
        private readonly int _idMensagem;
        public string userId;
        ViewCell lastCell;
        public RespostaPage()
        {
            InitializeComponent();

            userId = Application.Current.Properties["UserId"].ToString();
        }

        public RespostaPage(Mensagens mensagem)
        {
            InitializeComponent();
            CarregarMensagem(mensagem);

            _idMensagem = mensagem.Id;
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

        private async Task CarregarMensagem(Mensagens mensagem)
        {
            var pacienteMensagemResposta = await ObterMensagemResposta_Get(mensagem.Id);

            if ("Respondido" == mensagem.StatusMensagem || "Lido" == mensagem.StatusMensagem)
            {
                txtResposta.IsEnabled = false;
                btnEnviar.IsVisible = false;
                txtResposta.Text = pacienteMensagemResposta.Texto;
            }

            userId = Application.Current.Properties["UserId"].ToString();

            List<Mensagens> mensagemRespota = new List<Mensagens>();

            mensagemRespota.Add(mensagem);

            lstMensagens.ItemsSource = mensagemRespota;
        }

        private async Task<PacienteMensagemResposta> ObterMensagemResposta_Get(int id)
        {
            return await ApiService.GetMensagensRespostaByMensagemId(id);
        }

        private async void btnEnviar_Clicked(object sender, EventArgs e)
        {
            var textoResposta = txtResposta.Text;
            var apiResponse = MensagemResposta_Post(_idMensagem, textoResposta);

            if (!apiResponse.IsSucesso)
                btnEnviar.IsVisible = false;

            await Application.Current.MainPage.DisplayAlert("Aviso", "Mensagem enviada com sucesso!", "OK");

            //await Application.Current.MainPage.Navigation.PushAsync(new MensagemPage(), true);


            //alterado
            var page = new MensagemPage();
            this.Title = "Mensagem";
            PlaceHolder.Content = page.Content;

        }

        private ResponseMensage MensagemResposta_Post(int idMensagem, string texto)
        {
            return ApiService.PostMensagemResposta(idMensagem, texto);
        }
    }
}