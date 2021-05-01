using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoMRP.Paciente.ViewModel
{
    public class PlanoCuidado
    {
        public PlanoCuidado(){ }

        public string Descricao { get; set; }

        public bool IsAtivo { get; set; }

        public string Medico { get; set; }

        public string Especialidade { get; set; }

        public DateTime DataCriacao { get; set; }

        public int QtdCuidados { get; set; }

        public string DataMedico => $"{Medico} - {DataCriacao.ToString("dd/MM/yyyy HH:mm")}";
        public string QuantidadeHome => $"O paciente possui 2 planos de cuidados";
    }

    public class MsgQuantidade
    {
        public int QtdCuidados { get; set; }

        public string QuantidadeHome => $"O paciente possui {QtdCuidados} planos de cuidados";
        public MsgQuantidade()
        {

        }
    }
}
