using BotScanner._00___Setup;
using BotScanner._02___Utilidades;
using BotScanner._02___Utilidades.ConectaLa;
using BotScanner._02___Utilidades.Relatorio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.WebRequestMethods;

namespace BotScanner._01___Sellers
{
    public class Rovitex : Projeto
    {
        RestParametros restParametros = new();

        internal static string url = "https://www.rovitex.com.br/";
        internal static string rovitextEndpoint = "Api/V1/Products/list?store=78&status=enabled";
        internal static Produtos produtosCarregados = new();
        public static bool status;

        public Rovitex()
        {
            CarregarProdutos();
        }

        public static void FluxoRovitex()
        {
            MainWindow main = new();
            string planilhaRelatorio = PlanilhaPage.GerarPlanilha("Rovitex");

            IniciarNavegador();            

            foreach (var produto in produtosCarregados.result.data.Take(5))
            {
                main.AtualizarLog(produto.product.name);

                RealizarBusca(produto.product.sku);
                AcessarProduto();
                ValidarNome(produto.product.name);
            }           
            
            
            ValidarPreco();
            ValidarDescricao();
            ValidarPreco();
            ValidarDescricao();
            ValidarCor();

            produto.Status = status;
            PlanilhaPage.AtualizarPlanilha(planilhaRelatorio, produto);

            EncerrarNavegador();    
        }

        public static string FormatarSKU(string sku)
        {
            return sku.Replace("P_", "");
        }

        public static Produtos CarregarProdutos()
        {
            RestParametros parametrosRovitex = RestParametros.ConfiguracaoChamadaAPI();
            parametrosRovitex.Endpoint = rovitextEndpoint;

            string response = Rest.RealizarChamadaAPI(parametrosRovitex);
            produtosCarregados = JsonConvert.DeserializeObject<Produtos>(response);

            MainWindow.produtosSelecionados = produtosCarregados;

            return produtosCarregados;
        }

        public static void RealizarBusca(string termoBusca)
        {            
            NavegarPara($"https://www.rovitex.com.br/{FormatarSKU(termoBusca)}");
        }

        public static string AcessarProduto()
        {
            //RealizarClique_PorXpath("//*[@id=\"gallery-layout-container\"]/div[1]");            

            try
            {
                BuscarElemento_PorXpath("//*[@id=\"gallery-layout-container\"]/div/section/a").Click();
                status = true;
                return "Produto encontrado e clique efetuado com sucesso";
            }
            catch (Exception e)
            {
                status = false;
                Console.WriteLine(e);
                return $"Produto não encontrado";
            }
        }

        public static string ValidarNome(string nomeProdutoReferencia)
        {
            string nomeEncontradoProduto = BuscarTextoDoElemento_PorXpath("/html/body/div[5]/div/div[1]/div/div/div/div[4]/div/div[2]/div/section/div/div[2]/div/div/div/div[1]/div/div/div[1]/h1/span");           

            try
            {
                string texto = BuscarTextoDoElemento_PorXpath("/html/body/div[5]/div/div[1]/div/div/div/div[4]/div/div[2]/div/section/div/div[2]/div/div/div/div[1]/div/div/div[1]/h1/span").ToUpper();

                if (nomeEncontradoProduto.Equals(nomeProdutoReferencia))
                    status = true;
                return texto;
            }

            catch (Exception e)
            {
                status = false;                
                return $"Nome do item não encontrado na página";
            }
        }

        public static void ValidarPreco()
        {

        }

        public static void ValidarDescricao()
        {

        }

        public static void ValidarCor() 
        { 
        
        }

        public static void ValidarTamanho()
        {

        }        
    }
}

