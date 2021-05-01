using ProjetoMRP.Paciente.Services;
using ProjetoMRP.Paciente.ViewModel;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoMRP.Paciente.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            BindingContext = new LoginViewModel(Navigation);

        }

        //private async void btnLogin_Clicked(object sender, EventArgs e)
        //{
        //    MensagemLogin login = await ApiService.Login(txtUserName.Text, txtPassword.Text);

        //    if (login.IsLogin)
        //    {
        //        //await SecureStorage.SetAsync("UserId", Convert.ToString(login.UserId));
        //        Application.Current.Properties["UserId"] = login.UserId;
        //        await Navigation.PushAsync(new MainPage());
        //    }
        //    else
        //    {
        //        await DisplayAlert("Erro", login.Mensagem, "OK");
        //    }
        //}
    }
}