
using Microsoft.Practices.ServiceLocation;
using ProjetoMRP.Paciente.Configuration;

namespace ProjetoMRP.Paciente.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ContainerConfig.Configure();
        }

        public UserLoginViewModel UserLoginViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserLoginViewModel>();
            }
        }
    }
}
