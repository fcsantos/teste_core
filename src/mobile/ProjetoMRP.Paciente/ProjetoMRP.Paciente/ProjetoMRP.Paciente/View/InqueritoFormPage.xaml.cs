using ProjetoMRP.Paciente.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoMRP.Paciente.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InqueritoFormPage : ContentPage
    {
        public string userId;
        public InqueritoFormPage()
        {
            InitializeComponent();
            userId = Application.Current.Properties["UserId"].ToString();
            this.Title = "Inquerito Diabete";
        }

        public InqueritoFormPage(Inqueritos inquerito)
        {

        }
    }
}