﻿using BotScanner._00___Setup;
using BotScanner._02___Utilidades;
using BotScanner._02___Utilidades.ConectaLa;
using BotScanner._02___Utilidades.Relatorio;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
        public static bool precoComAjustePercentual;
        public static List<bool> listaDeStatus = new();

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

            

            IniciarNavegador();

            //Ajuste de quantidade de produtos
            foreach (var produto in produtosCarregados.result.data.Take(100))
            {
                PlanilhaPage produtoValidado = new();

                RealizarBusca(produto.product.sku);
                //RealizarBusca("61127");
                
                AcessarProduto();

                produtoValidado.Seller = seller;
                produtoValidado.SKU_Parceiro = produto.product.sku;
                produtoValidado.LinkBusca = RetornarUrlSeller(produto.product.sku);
                produtoValidado.LinkConectaLa = RetornarUrlConectaLa(produto.product.product_id);

                var nome = ValidarNome(produto.product.name, produtoValidado);
                var preco = ValidarPreco(produto.product.price, produtoValidado);
                var desc = ValidarDescricao(produto.product.description, produtoValidado);
                
                var cor = ValidarCor(produto.product.variations[0].variant[1].color, produto.product.name, produtoValidado);
                var tamanho = ValidarTamanho(produto.product.variations, produtoValidado);


                produtoValidado.Status = RetornarStatusConsolidado(listaDeStatus).ToString();

                PlanilhaPage.AtualizarPlanilha(planilhaRelatorio, produtoValidado);
                
                await main.AtualizarLogAsync(produto.product.name, produtoValidado.Status);
                
                await Task.Delay(100);

                listaDeStatus = new();
            }
            
                                    
            

            EncerrarNavegador();
        }
        

        public static bool RetornarStatusConsolidado(List<bool> listaStatus)
        {
            if (listaStatus.Contains(false))
                return false;
            else
                return true;
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

                listaDeStatus.Add(status);
                return status;
            }

            catch (Exception e)
            {
                produtoValidado.NomeEncontrado = "Produto não encontrado";
                produtoValidado.NomeEsperado = nomeEsperadoProduto;
                listaDeStatus.Add(status);
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

                //if (precoEncontradoProduto.Equals(precoEsperadoProduto))
                //    status = true;
                //else
                //    status = ValidarPrecoComIntervalo(precoEncontradoProduto, precoEsperadoProduto);

                if (precoEncontradoProduto.Equals(precoEsperadoProduto))
                    status = true;
                else
                    status = false;

                produtoValidado.PrecoEncontrado = precoEncontradoProduto.Replace(",",".");
                produtoValidado.PrecoEsperado = precoEsperadoProduto.Replace(",", ".");

                listaDeStatus.Add(status);
                return status;
            }

            catch (Exception e)
            {
                produtoValidado.PrecoEncontrado = "Produto não encontrado";
                produtoValidado.PrecoEsperado = precoEsperadoProduto.Replace(",", ".");
                listaDeStatus.Add(status);
                return status = false;
            }
        }


        public static bool ValidarPrecoComIntervalo(string precoEncontrado, string precoEsperado)
        {
            if (float.TryParse(precoEncontrado.Replace(",", "."), out float precoEncontradoFloat) &&
                float.TryParse(precoEsperado.Replace(",", "."), out float precoEsperadoFloat))
            {
                float minimoPermitido = precoEsperadoFloat * 0.90f;
                float maximoPermitido = precoEsperadoFloat * 1.10f;

                bool precoValidado = precoEncontradoFloat >= minimoPermitido && precoEncontradoFloat <= maximoPermitido;
                if (precoValidado)
                    precoComAjustePercentual = true;

                return precoValidado;
            }
            return false;
        }



        public static bool ValidarDescricao(string descricaoProdutoReferencia, PlanilhaPage produtoValidado)
        {
            string descricaoEncontradoProduto = string.Empty;
            string descricaoEsperadoProduto = StringFormatter.FormatarTexto_Descricao(descricaoProdutoReferencia);

            try
            {
                descricaoEncontradoProduto = StringFormatter.FormatarTexto_Descricao(BuscarTextoDoElemento_PorXpath("/html/body/div[5]/div/div[1]/div/div/div/div[4]/div/div[2]/div/section/div/div[2]/div/div/div/div[12]/div[1]/div/div/div/div/div"));

                if (descricaoEncontradoProduto.Equals(descricaoEsperadoProduto))
                    status = true;
                else
                    status = false;


                produtoValidado.DescricaoEncontrada = descricaoEncontradoProduto;
                produtoValidado.DescricaoEsperada = descricaoEsperadoProduto;

                listaDeStatus.Add(status);
                return status;
            }

            catch (Exception e)
            {
                produtoValidado.DescricaoEncontrada = "Produto não encontrado";
                produtoValidado.DescricaoEsperada = descricaoEsperadoProduto;
                listaDeStatus.Add(status);
                return status = false;
            }
        }


        public static bool ValidarCor(string corProdutoReferencia, string corNomeProduto, PlanilhaPage produtoValidado) 
        {
            string corEncontradaProduto = string.Empty;
            string corEsperadaProduto = corProdutoReferencia;
            

            try
            {
                corEncontradaProduto = corNomeProduto;                

                if (corEncontradaProduto.Contains(corEsperadaProduto))
                    status = true;
                else
                    status = false;


                produtoValidado.CoresEncontradas = ExtrairCorDaString(corEncontradaProduto, corProdutoReferencia);
                produtoValidado.CoresEsperadas = corEsperadaProduto;

                listaDeStatus.Add(status);
                return status;
            }

            catch (Exception e)
            {
                if (string.IsNullOrEmpty(corEncontradaProduto) && string.IsNullOrEmpty(corProdutoReferencia))
                {
                    produtoValidado.CoresEncontradas = "Produto sem cor no site";
                    produtoValidado.CoresEsperadas = corEsperadaProduto;
                    return status = true;
                }
                else
                {
                    produtoValidado.CoresEncontradas = "Produto não encontrado";
                    produtoValidado.CoresEsperadas = corEsperadaProduto;
                    listaDeStatus.Add(status);
                    return status = false;
                }                
            }
        }

        public static string ExtrairCorDaString(string texto, string cor)
        {
            int startIndex = texto.IndexOf(cor, StringComparison.OrdinalIgnoreCase);
            if (startIndex != -1)
            {
                int length = cor.Length;
                return texto.Substring(startIndex, length);
            }
            return string.Empty; 
        }

        public static bool ValidarTamanho(List<Variation> variations, PlanilhaPage produtoValidado)
        {
            List<string> listaTamanhosEsperados = new List<string>();
            List<string> listaTamanhosEncontrados = new List<string>();

            var oikkk = BuscarListaDeElementos_PorCssSelector(".vtex-store-components-3-x-skuSelectorSelectorImageValue");            
            
            try
            {
                var tamanhoProdutoLista = BuscarListaDeElementos_PorCssSelector(".vtex-store-components-3-x-skuSelectorItemTextValue");                

                foreach (var variation in variations)
                {
                    foreach (var item in variation.variant)
                    {
                        if (!string.IsNullOrEmpty(item.size))                        
                            listaTamanhosEsperados.Add(item.size);
                    }
                }

                foreach (var tamanho in tamanhoProdutoLista)
                {
                    listaTamanhosEncontrados.Add(tamanho.Text);
                }

                listaTamanhosEsperados.Sort(StringComparer.OrdinalIgnoreCase);
                listaTamanhosEncontrados.Sort(StringComparer.OrdinalIgnoreCase);

                produtoValidado.TamanhoEncontrado = String.Join(",", listaTamanhosEncontrados);
                produtoValidado.TamanhoEsperado = String.Join(",", listaTamanhosEsperados);

                status = CompararListasOrdenadas(listaTamanhosEsperados, listaTamanhosEncontrados);

                listaDeStatus.Add(status);
                return status;
            }

            catch (Exception e)
            {
                produtoValidado.TamanhoEncontrado = "Produto não encontrado";
                produtoValidado.TamanhoEsperado = String.Join(",", listaTamanhosEsperados);
                listaDeStatus.Add(status);
                return status = false;
            }
        }

        public static bool CompararListasOrdenadas(List<string> listaTamanhosEsperados, List<string> listaTamanhosEncontrados)
        {           

            if (listaTamanhosEsperados.Count != listaTamanhosEncontrados.Count)
                return false;

            for (int i = 0; i < listaTamanhosEsperados.Count; i++)
            {
                if (!listaTamanhosEsperados[i].Equals(listaTamanhosEncontrados[i], StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return true;
        }
    }
}

