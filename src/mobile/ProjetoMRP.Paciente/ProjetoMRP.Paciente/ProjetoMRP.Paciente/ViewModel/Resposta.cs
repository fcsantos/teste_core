using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoMRP.Paciente.ViewModel
{
    public class Resposta : Mensagens
    {
        public int IdResposta { get; set; }

        public string Descricao { get; set; }

        public DateTime dtResposta { get; set; }
    }
}
