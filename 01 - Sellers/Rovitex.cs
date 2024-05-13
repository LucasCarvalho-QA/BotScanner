using BotScanner._00___Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BotScanner._01___Sellers
{
    public class Rovitex : Projeto
    {
        internal static string url = "https://www.rovitex.com.br/";

        public static void FluxoRovitex()
        {
            AcessarSite();

            RealizarBusca();
            AcessarProduto();
            ValidarNome();
            ValidarPreco();
            ValidarDescricao();
            ValidarPreco();
            ValidarDescricao();
            ValidarCor();

            EncerrarNavegador();    
        }

        public static void AcessarSite()
        {
            IniciarNavegador(url);
        }

        public static void RealizarBusca()
        {

        }

        public static void AcessarProduto()
        {

        }

        public static void ValidarNome()
        {

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

