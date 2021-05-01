using ProjetoMRP.Paciente.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;



namespace ProjetoMRP.Paciente.ViewModel
{
    public class LoginViewModel
    {
        public LoginViewModel(INavigation nav)
        {
            LoginCommand = new Command(LoginCommandAction);
            Navigation = nav;
#if DEBUG
            username = "admin@admin.com";
            password = "Admin@123456";
#endif
        }

        public Command LoginCommand { get; set; }
        public INavigation Navigation { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        private void LoginCommandAction()
        {
            Application.Current.Properties["UserId"] = "";
            var auth = new ApiService();

            var authVm = new AuthenticationViewModel
            {
                Email = username,
                Password = password
            };

            //Task.Run(async () =>
            //{
            auth.Login(authVm).Wait();


            //});

            //await _authenticationService.AccomplishLogin(response);

            //Application.Current.MainPage.Navigation.PushAsync(new MainPage());

            if (Application.Current.Properties != null && Application.Current.Properties["UserId"] != "")
                Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new MainPage());
            else
                Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Erro", "Login invalido", "OK");

        }
    }

    public class AuthenticationViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}