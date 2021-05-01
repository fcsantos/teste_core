using ProjetoMRP.Paciente.Services;
using ProjetoMRP.Paciente.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoMRP.Paciente.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlanoCuidados : ContentPage
    {
        public string userId;
        ViewCell lastCell;
        public PlanoCuidados()
        {
            userId = Application.Current.Properties["UserId"].ToString();
            InitializeComponent();
            CarregarLista(userId);
            CarregarListaPlanoCuidados(userId);
        }
        private async void CarregarLista(string userId)
        {
            List<PlanoCuidado> planoCuidados = await ApiService.GetPlanoCuidadosByIdUser(Convert.ToInt32(userId));
            lst.ItemsSource = planoCuidados;
        }
        private async void CarregarListaPlanoCuidados(string userId)
        {
            List<PlanoCuidado> planoCuidados = await ApiService.GetPlanoCuidadosByIdUser(Convert.ToInt32(userId));

            lst.ItemsSource = planoCuidados;
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