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
        public float offset { get; set; }
        public Result result { get; set; }

    }

    public class Result
    {
        public bool error { get; set; }
        public int registers_count { get; set; }
        public float pages_count { get; set; }
        public float page { get; set; }
        public List<Datum>? data { get; set; }
    }

    public class Datum
    {
        public Product product { get; set; }
    }

    public class Product
    {
        public string sku { get; set; }
        public string name { get; set; }
        public string product_id { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public float qty { get; set; }
        public float price { get; set; }
        public float weight_gross { get; set; }
        public float weight_liquid { get; set; }
        public float height { get; set; }
        public float width { get; set; }
        public float length { get; set; }
        public string items_per_package { get; set; }
        public string brand { get; set; }
        public string ean { get; set; }
        public object ncm { get; set; }
        public List<Category> categories { get; set; }
        public List<string> images { get; set; }
        public List<string> variation_attributes { get; set; }
    }

    public class Category
    {
        public string code { get; set; }
        public string name { get; set; }
    }    
}
