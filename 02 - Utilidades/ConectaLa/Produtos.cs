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
    }

    public class Result
    {
        public bool error { get; set; }
        public int registers_count { get; set; }
        public int pages_count { get; set; }
        public int page { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Datum
    {
        public Product product { get; set; }
    }

    public class Product
    {
        public string store_id { get; set; }
        public string product_id { get; set; }
        public string sku { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public int qty { get; set; }
        public float price { get; set; }
        public float list_price { get; set; }
        public float weight_gross { get; set; }
        public float weight_liquid { get; set; }
        public float height { get; set; }
        public int width { get; set; }
        public int length { get; set; }
        public int items_per_package { get; set; }
        public string brand { get; set; }
        public long ean { get; set; }
        public int ncm { get; set; }
        public List<Category> categories { get; set; }
        public List<string> images { get; set; }
        public List<Variation> variations { get; set; }
        public string[] variation_attributes { get; set; }
    }

    public class Category
    {
        public int code { get; set; }
        public string name { get; set; }
    }

    public class Variation
    {
        public string variant_id { get; set; }
        public string sku { get; set; }
        public int qty { get; set; }
        public long EAN { get; set; }
        public float price { get; set; }
        public float list_price { get; set; }
        public string[] images { get; set; }
        public List<Variant> variant { get; set; }
    }

    public class Variant
    {
        public string size { get; set; }
        public string color { get; set; }
    }


}


