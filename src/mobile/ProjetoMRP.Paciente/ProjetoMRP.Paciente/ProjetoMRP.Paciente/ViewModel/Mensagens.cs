using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoMRP.Paciente.ViewModel
{
    public class Mensagens
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public int StatusMensagemId { get; set; }
        public string StatusMensagem { get; set; }
        public int PacienteId { get; set; }
        public string NovoPaciente { get; set; }
        public int MedicoId { get; set; }
        public string NomeMedico { get; set; }
        public int MensagemRespostaId { get; set; }
        public string Data { get; set; }
        public bool IsResponder { get; set; }
    }
}
