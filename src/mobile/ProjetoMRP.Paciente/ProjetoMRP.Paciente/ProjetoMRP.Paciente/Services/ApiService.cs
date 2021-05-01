using Newtonsoft.Json;
using ProjetoMRP.Paciente.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProjetoMRP.Paciente.Services
{
    public class ApiService : Services
    {
        public const string url = "http://192.168.1.198:5000";
        static readonly HttpClient client = new HttpClient();

        public static async Task<List<Especialidade>> GetEspecialidades()
        {
            try
            {
                HttpClient client = new HttpClient();
                string urlComplete = url + "RESTAPI_Especialidade/RESTAPIGetAll";
                string response = await client.GetStringAsync(urlComplete);
                List<Especialidade> especialidades = JsonConvert.DeserializeObject<List<Especialidade>>(response);

                return especialidades;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Login(AuthenticationViewModel login)
        {
#if DEBUG
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (o, cert, chain, errors) => true
            };

#else
            var httpHandler = new HttpClientHandler();

#endif
            using (var client = new HttpClient(httpHandler))
            {

                var loginContent = GetContent(login);

                string urlComplete = url + "/api/v1/login";
                var response = client.PostAsync(new Uri(urlComplete), loginContent).Result;

                //var param = response.Content.ReadAsStringAsync();

                //var retornoMensagem = new UserResponseLogin
                //{
                //    ResponseResult = await DeserializedObjectResponse<ResponseResult>(response)
                //};

                var userResponse = await DeserializedObjectResponse<UserResponseLogin>(response);
                if (userResponse != null && !string.IsNullOrEmpty(userResponse.AccessToken))
                {
                    Application.Current.Properties["AccessToken"] = userResponse.AccessToken;
                    Application.Current.Properties["UserId"] = userResponse.UserToken.Id;
                    Application.Current.Properties["User"] = userResponse.UserToken;
                }


            }



        }

        public static async Task<List<PlanoCuidado>> GetPlanoCuidadosByIdUser(int userId)
        {
            try
            {
                var msgQuantidade = new MsgQuantidade();
                HttpClient client = new HttpClient();
                string urlComplete = url + "RESTAPI_PlanoCuidado/RESTAPIGetAllByUserId";
                string response = await client.GetStringAsync(new Uri(urlComplete + "?UserId=" + userId));
                List<PlanoCuidado> planoCuidadosList = JsonConvert.DeserializeObject<List<PlanoCuidado>>(response);

                var qtd = planoCuidadosList.Count();
                foreach (var item in planoCuidadosList)
                    item.QtdCuidados = qtd;

                var teste = msgQuantidade.QuantidadeHome;

                return planoCuidadosList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<Inqueritos>> GetInqueritosByIdUser(int userId)
        {
            try
            {
                HttpClient client = new HttpClient();
                string urlComplete = url + "RESTAPI_Inqueritos/RESTAPIGetAllByUserId";
                string response = await client.GetStringAsync(new Uri(urlComplete + "?UserId=" + userId));
                List<Inqueritos> inqueritos = JsonConvert.DeserializeObject<List<Inqueritos>>(response);

                return inqueritos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<Mensagens>> GetMensagensByIdUser(int userId)
        {
            try
            {
                HttpClient client = new HttpClient();
                string urlComplete = url + "RESTAPI_Mensagem/RESTAPIGetAllByUserId";
                string response = await client.GetStringAsync(new Uri(urlComplete + "?UserId=" + userId));
                List<Mensagens> mensagens = JsonConvert.DeserializeObject<List<Mensagens>>(response);

                return mensagens;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<PacienteMensagemResposta> GetMensagensRespostaByMensagemId(int mensagemId)
        {
            try
            {
                HttpClient client = new HttpClient();
                string urlComplete = url + "RESTAPI_Mensagem/RESTAPIMensagemRespostaById?MensagemId=" + mensagemId;
                string response = await client.GetStringAsync(new Uri(urlComplete));
                var mensagens = JsonConvert.DeserializeObject<PacienteMensagemResposta[]>(response);
                var resposta = mensagens.FirstOrDefault(m => m.MensagemRespostaId == mensagemId);

                return resposta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static ResponseMensage PostMensagemResposta(int mensagemId, string texto)
        {
            try
            {
                var retornoMensagem = new ResponseMensage();
                var data = new TextoResposta();
                data.Texto = texto;


                HttpClient client = new HttpClient();


                var jsonString = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                string urlComplete = url + "RESTAPI_Mensagem/RESTAPIMensagemResposta";
                var ApiResponse = client.PostAsync(new Uri(urlComplete + "?MensagemId=" + mensagemId), content);
                var resultado = ApiResponse.Result;
                var param = resultado.Content.ReadAsStringAsync().Result;

                if (resultado.IsSuccessStatusCode)
                {
                    retornoMensagem = JsonConvert.DeserializeObject<ResponseMensage>(param);
                }

                return new ResponseMensage
                {
                    Mensagem = retornoMensagem.Mensagem,
                    IsSucesso = retornoMensagem.IsSucesso
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public class ResponseMensage
    {
        public string Mensagem { get; set; }
        public bool IsSucesso { get; set; }
    }

    public class TextoResposta
    {
        public string Texto { get; set; }
    }


    public class Rootobject
    {
        public PacienteMensagemResposta[] PacienteMensagemResposta { get; set; }
    }

    public class PacienteMensagemResposta
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public int StatusMensagemId { get; set; }
        public string StatusMensagem { get; set; }
        public int PacienteId { get; set; }
        public string NomePaciente { get; set; }
        public int MedicoId { get; set; }
        public string NomeMedico { get; set; }
        public string Data { get; set; }
        public string DataResposta { get; set; }
        public int MensagemRespostaId { get; set; }
        public string MensagemResposta { get; set; }
    }
}
