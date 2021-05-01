using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoMRP.Paciente.ViewModel
{
    public class MensagemLogin
    {
        public string Mensagem { get; set; }

        public bool IsLogin { get; set; }

        public int UserId { get; set; }

        public string NomePaciente { get; set; }
    }
}
