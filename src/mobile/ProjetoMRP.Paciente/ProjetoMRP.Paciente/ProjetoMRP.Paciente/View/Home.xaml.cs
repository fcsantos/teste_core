using ProjetoMRP.Paciente.Services;
using ProjetoMRP.Paciente.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoMRP.Paciente.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public string userId;
        public Home()
        {
            userId = Application.Current.Properties["UserId"].ToString();
            
            if (string.IsNullOrEmpty(userId))
                Navigation.PushAsync(new LoginPage());

            InitializeComponent();
            
            CarregarMsgHome(userId);
        }
        #region Métodos Home
        private async void CarregarMsgHome(string userId)
        {
            await PlanoCuidadosHome(userId);
            await InqueritosHome(userId);
            await MensagensHome(userId);
        }
        private async Task PlanoCuidadosHome(string userId)
        {
            /*
             * List<PlanoCuidado> planoCuidados = await ApiService.GetPlanoCuidadosByIdUser(Convert.ToInt32(userId));

            var qtd = planoCuidados.Count();
            */
            lblPlanoCuidados.Text = $"Você possui 000 plano(s) de cuidado(s).";
        }
        private async Task InqueritosHome(string userId)
        {
            /*List<Inqueritos> inqueritos = await ApiService.GetInqueritosByIdUser(Convert.ToInt32(userId));

            var qtdInqueritos = inqueritos.Count();
            */
            lblInqueritos.Text = $"Você possui 00000 inquérito(s).";
        }
        private async Task MensagensHome(string userId)
        {
            /*List<Mensagens> mensagens = await ApiService.GetMensagensByIdUser(Convert.ToInt32(userId));

            var qtdMensagens = mensagens.Count();
            */
            lblMensagem.Text = $"Você possui 0000 mensagem(s) não lida(s).";
        }
        #endregion

        #region Eventos
        private async void btnPlanoCuidados_Clicked(object sender, System.EventArgs e)
        {
            //await Navigation.PushAsync(new PlanoCuidados());
            await Navigation.PushAsync(new PlanoCuidados());
        }

        private async void btnInqueritos_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new InqueritosPage());
        }

        private async void btnMessagens_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MensagemPage());
        }

        private async void btnAjuda_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AjudaPage());
        }
        #endregion
    }
}