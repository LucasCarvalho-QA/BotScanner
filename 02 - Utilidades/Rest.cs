using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotScanner._02___Utilidades
{
    public class RestParametros
    {
        public Method Metodo { get; set; }
        public string BaseURL {  get; set; }
        public string Endpoint { get; set; }        
        public string UserEmail { get; set; }
        public string XEmail { get; set; }
        public string ApiKey { get; set; }
        public string ProviderKey { get; set; }

        public static RestParametros ConfiguracaoChamadaAPI() 
        {
            return new RestParametros 
            { 
                Metodo = Method.Get,
                Endpoint = string.Empty,                
                BaseURL = "https://privaliamarketplace.conectala.com.br/app/",
                UserEmail = "api@privalia.com",
                ApiKey = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJwcm92aWRlcl9pZCI6IjEiLCJlbWFpbCI6ImFwaUBwcml2YWxpYS5jb20ifQ.pZuGeku8WM3aGmDJ7ZJtEAZ5pzpqn8HWBqqP71JUIgA",
                ProviderKey = "1",
                XEmail = "api@privalia.com"
            };
        }
    }

    public class Rest
    {
        public static async Task<string> RealizarChamadaAPIAsync(RestParametros parametros)
        {
            RestClient client = new RestClient(parametros.BaseURL);

            RestRequest request = new RestRequest(parametros.Endpoint, parametros.Metodo);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("accept", "application/json;charset=UTF-8");
            request.AddHeader("x-user-email", parametros.UserEmail);
            request.AddHeader("x-api-key", parametros.ApiKey);
            request.AddHeader("x-provider-key", parametros.ProviderKey);
            request.AddHeader("x-email", parametros.XEmail);

            var response = await client.ExecuteAsync(request);
            return response.Content;
        }

 
    }
}
