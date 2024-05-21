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
            string seller = "Rovitex";
            string planilhaRelatorio = PlanilhaPage.GerarPlanilha(seller);

            PlanilhaPage produtoValidado = new();

            IniciarNavegador();

            foreach (var produto in produtosCarregados.result.data.Take(7))
            {
                RealizarBusca(produto.product.sku);
                
                AcessarProduto();

                ValidarNome(produto.product.name, produtoValidado);
                ValidarPreco(produto.product.price, produtoValidado);
                ValidarDescricao(produto.product.description, produtoValidado);

                produtoValidado.Seller = seller;
                produtoValidado.SKU_Parceiro = produto.product.sku;
                produtoValidado.LinkBusca = RetornarUrlSeller(produto.product.sku);
                produtoValidado.LinkConectaLa = RetornarUrlConectaLa(produto.product.product_id);
                produtoValidado.Status = status.ToString();

                PlanilhaPage.AtualizarPlanilha(planilhaRelatorio, produtoValidado);
                
                await main.AtualizarLogAsync(produto.product.name, produtoValidado.Status);
                
                await Task.Delay(100);
            }
            
                                    
            ValidarCor();

            EncerrarNavegador();
        }


        public static string FormatarSKU(string sku)
        {
            return sku.Replace("P_", "");
        }

        public static string RetornarUrlConectaLa(string produtoId)
        {
            return $"https://privaliamarketplace.conectala.com.br/app/products/update/{produtoId}";
        }

        public static string RetornarUrlSeller(string termoBusca)
        {
            return $"{urlBase}{FormatarSKU(termoBusca)}";
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

        public static bool ValidarPreco(float precoReferencia, PlanilhaPage produtoValidado)
        {
            string precoEncontradoProduto = string.Empty;
            string precoEsperadoProduto = precoReferencia.ToString();

            try
            {                
                string realInteiro = BuscarTextoDoElemento_PorXpath("/html/body/div[5]/div/div[1]/div/div/div/div[4]/div/div[2]/div/section/div/div[2]/div/div/div/div[4]/div/div/div[2]/span/span/span/span[3]");
                string realCentavos = BuscarTextoDoElemento_PorXpath("/html/body/div[5]/div/div[1]/div/div/div/div[4]/div/div[2]/div/section/div/div[2]/div/div/div/div[4]/div/div/div[2]/span/span/span/span[5]");
                precoEncontradoProduto = $"{realInteiro},{realCentavos}";

                if (precoEncontradoProduto.Equals(precoEsperadoProduto))
                    status = true;
                else
                    status = false;


                produtoValidado.PrecoEncontrado = precoEncontradoProduto.Replace(",",".");
                produtoValidado.PrecoEsperado = precoEsperadoProduto.Replace(",", ".");

                return status;
            }

            catch (Exception e)
            {
                produtoValidado.PrecoEncontrado = "Preço não correspondente";
                produtoValidado.PrecoEsperado = precoEsperadoProduto.Replace(",", ".");
                return status = false;
            }
        }

        public static bool ValidarDescricao(string descricaoProdutoReferencia, PlanilhaPage produtoValidado)
        {
            string descricaoEncontradoProduto = string.Empty;
            string descricaoEsperadoProduto = StringFormatter.FormatarTexto_Descricao(descricaoProdutoReferencia);

            try
            {
                descricaoEncontradoProduto = BuscarTextoDoElemento_PorXpath("/html/body/div[5]/div/div[1]/div/div/div/div[4]/div/div[2]/div/section/div/div[2]/div/div/div/div[12]/div[1]/div/div/div/div/div");

                if (descricaoEncontradoProduto.Equals(descricaoProdutoReferencia))
                    status = true;
                else
                    status = false;


                produtoValidado.DescricaoEncontrada = descricaoEncontradoProduto;
                produtoValidado.DescricaoEsperada = descricaoEsperadoProduto;

                return status;
            }

            catch (Exception e)
            {
                produtoValidado.DescricaoEncontrada = "Descrição não correspondente";
                produtoValidado.DescricaoEsperada = descricaoEsperadoProduto;
                return status = false;
            }
        }


        public static void ValidarCor() 
        { 
        
        }

        public static void ValidarTamanho()
        {

        }        
    }
}

