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

        internal static string urlBase = "https://www.rovitex.com.br/";
        internal static string rovitextEndpoint = "Api/V1/Products/list?store=78&status=enabled";
        internal static Produtos produtosCarregados = new();
        public static bool status;

        public Rovitex()
        {
            _ = CarregarProdutosAsync();
        }

        public async Task CarregarProdutosAsync()
        {
            await CarregarProdutos();
        }
        
        public static async Task<Produtos> CarregarProdutos()
        {
            RestParametros parametrosRovitex = RestParametros.ConfiguracaoChamadaAPI();
            parametrosRovitex.Endpoint = rovitextEndpoint;

            string response = await Rest.RealizarChamadaAPIAsync(parametrosRovitex);
            var produtosDesserializados = JsonConvert.DeserializeObject<Produtos>(response);                        
            
            produtosCarregados = produtosDesserializados;            
            MainWindow.quantidadeTotalItens = produtosCarregados.result.data.Count;
            MainWindow.produtosSelecionados = produtosCarregados;

            return produtosCarregados;
        }


        public static async Task FluxoRovitex(MainWindow main)
        {
            string planilhaRelatorio = PlanilhaPage.GerarPlanilha("Rovitex");

            PlanilhaPage produtoValidado = new();

            IniciarNavegador();

            foreach (var produto in produtosCarregados.result.data.Take(7))
            {
                string termoBusca = RealizarBusca(produto.product.sku);
                AcessarProduto();

                ValidarNome(produto.product.name, produtoValidado);

                produtoValidado.Seller = "Rovitex";
                produtoValidado.SKU_Parceiro = produto.product.sku;
                produtoValidado.LinkBusca = termoBusca;

                produtoValidado.Status = status.ToString();

                PlanilhaPage.AtualizarPlanilha(planilhaRelatorio, produtoValidado);
                
                await main.AtualizarLogAsync(produto.product.name, produtoValidado.Status);

                // Adiciona um pequeno atraso para permitir a atualização da UI
                await Task.Delay(100);
            }

            ValidarPreco();
            ValidarDescricao();
            ValidarPreco();
            ValidarDescricao();
            ValidarCor();

            EncerrarNavegador();
        }


        public static string FormatarSKU(string sku)
        {
            return sku.Replace("P_", "");
        }

        

        public static string RealizarBusca(string termoBusca)
        {            
            NavegarPara($"{urlBase}{FormatarSKU(termoBusca)}");

            return $"{urlBase}{FormatarSKU(termoBusca)}";
        }

        public static string AcessarProduto()
        {
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

        public static bool ValidarNome(string nomeProdutoReferencia, PlanilhaPage produtoValidado)
        {
            string nomeEncontradoProduto = string.Empty;
            string nomeEsperadoProduto = nomeProdutoReferencia;

            try
            {
                nomeEncontradoProduto = BuscarTextoDoElemento_PorXpath("/html/body/div[5]/div/div[1]/div/div/div/div[4]/div/div[2]/div/section/div/div[2]/div/div/div/div[1]/div/div/div[1]/h1/span");

                if (nomeEncontradoProduto.Equals(nomeProdutoReferencia))
                    status = true;
                else                
                    status = false;
                

                produtoValidado.NomeEncontrado = nomeEncontradoProduto;
                produtoValidado.NomeEsperado = nomeEsperadoProduto;

                return status;
            }

            catch (Exception e)
            {
                produtoValidado.NomeEncontrado = "Produto não encontrado";
                produtoValidado.NomeEsperado = nomeEsperadoProduto;
                return status = false;                                
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

