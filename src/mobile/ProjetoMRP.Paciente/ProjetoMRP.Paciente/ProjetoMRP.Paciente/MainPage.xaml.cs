using ProjetoMRP.Paciente.View;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoMRP.Paciente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public string userId;
        public  MainPage()
        {
            InitializeComponent();
            userId = Application.Current.Properties["UserId"].ToString();
            NavigationPage.SetHasBackButton(this, false);

            var page = new Home();
            this.Title = "Início";
            PlaceHolder.Content = page.Content;
        }

        void Icon1_Tapped(object sender,System.EventArgs e)
        {
            var page = new PlanoCuidados();

            this.Title = "Plano de Cuidados";

            PlaceHolder.Content = page.Content;
        }

        void Icon2_Tapped(object sender, System.EventArgs e)
        {
            var page = new InqueritosPage();
            this.Title = "Inquéritos";
            PlaceHolder.Content = page.Content;
        }

        void Icon3_Tapped(object sender, System.EventArgs e)
        {
            var page = new Home();
            this.Title = "Início";
            PlaceHolder.Content = page.Content;
        }

        void Icon4_Tapped(object sender, System.EventArgs e)
        {
            var page = new MensagemPage();
            this.Title = "Mensagem";
            PlaceHolder.Content = page.Content;
        }
        void Icon5_Tapped(object sender, System.EventArgs e)
        {
            var page = new AjudaPage();
            this.Title = "Ajuda";
            PlaceHolder.Content = page.Content;
        }

        void ShowUserProfile(object sender, System.EventArgs e)
        {
            Application.Current.Properties.Clear();
            Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
    }
}
