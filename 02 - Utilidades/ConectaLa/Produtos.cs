using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotScanner._02___Utilidades.ConectaLa
{
    public class Produtos
    {
        public bool success { get; set; }
        public int offset { get; set; }
        public Result result { get; set; }

        public static Produtos? SelecionarProdutos()
        {
            RestParametros parametros = new RestParametros().ConfiguracaoChamadaAPI();
            parametros.Endpoint = "Api/V1/Products/";
            string response = Rest.RealizarChamadaAPI(parametros);

            return JsonConvert.DeserializeObject<Produtos>(response);
        }
    }

    public class Result
    {
        public bool error { get; set; }
        public int registers_count { get; set; }
        public int pages_count { get; set; }
        public int page { get; set; }
        public Datum[] data { get; set; }
    }

    public class Datum
    {
        public Product product { get; set; }
    }

    public class Product
    {
        public string sku { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public int qty { get; set; }
        public float price { get; set; }
        public float weight_gross { get; set; }
        public float weight_liquid { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public int length { get; set; }
        public string items_per_package { get; set; }
        public string brand { get; set; }
        public string ean { get; set; }
        public object ncm { get; set; }
        public Category[] categories { get; set; }
        public string[] images { get; set; }
        public string[] variation_attributes { get; set; }
    }

    public class Category
    {
        public string code { get; set; }
        public string name { get; set; }
    }    
}
