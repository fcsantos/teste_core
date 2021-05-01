using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoMRP.Paciente.ViewModel
{
    public class Inqueritos
    {
        public string NomeInquerito { get; set; }
        public string Data { get; set; }
        public string Medico { get; set; }
        public string Patologia { get; set; }
        public string Status { get; set; }

        public string DataInquerito => $"{NomeInquerito} - {Data}";
        public string DataCriacaoInquerito => $"Data criação: {Data}";
        public string DataModificacaoInquerito => $"Data modificação: 01/03/2021";
    }
}
