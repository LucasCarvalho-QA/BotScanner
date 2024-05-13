using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotScanner._02___Utilidades
{
    public class RestParametros
    {
        public Method Metodo { get; set; }
        public string? BaseURL {  get; set; }
        public string? Endpoint { get; set; }        
        public string? UserEmail { get; set; }
        public string? ApiKey { get; set; }
        public string? StoreKey { get; set; }

        public RestParametros ConfiguracaoChamadaAPI() 
        {
            return new RestParametros 
            { 
                Metodo = Method.Get,
                Endpoint = string.Empty,                
                BaseURL = "https://3f9af969-e3d5-4abc-943a-d0c3ad4f1905.mock.pstmn.io/app/",
                UserEmail = "your-email@example.com",
                ApiKey = "your-api-key",
                StoreKey = "tour-store-key",
            };
        }
    }

    public class Rest
    {   
        public static string RealizarChamadaAPI(RestParametros parametros)
        {
            RestClient client = new(parametros.BaseURL);

            RestRequest request = new(parametros.Endpoint, parametros.Metodo);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("accept", "application/json;charset=UTF-8");
            request.AddHeader("x-user-email", parametros.UserEmail);
            request.AddHeader("x-api-key", parametros.ApiKey);
            request.AddHeader("x-store-key", parametros.StoreKey);            

            var response = client.Execute(request);            
            return response.Content;
        }
    }
}
