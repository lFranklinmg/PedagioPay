using PedagioPayPortalUsuario.Data.Dtos.AutenticacaoExterna;
using PedagioPayPortalUsuario.Service.Interfaces;
using System.Net;
using System.Net.Http;

namespace PedagioPayPortalUsuario.Service
{
    public class AutenticacaoExternaService : IAutenticacaoExterna
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public async Task<string> GetUsuario (string url) 
        {
            try
            {
                url = "";
                var resultado = await HttpClient.GetAsync(url);
                var retorno = await resultado.Content.ReadAsStringAsync();
                Console.WriteLine(retorno);
                return retorno;
            }
            catch (Exception ex)
            {

            }
            return "Erro";
        }
    }
}
