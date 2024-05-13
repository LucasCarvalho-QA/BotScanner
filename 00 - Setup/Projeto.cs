using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotScanner._00___Setup
{
    
    public class Projeto : WebDriver
    {
        
        public static void IniciarNavegador(string url)
        {
            ParametrosWebDriver parametros = new() 
            { 
                Headless = false, 
                TempoEspera = 5 
            };

            ConfigurarWebDriverChrome(parametros);
            NavegarPara(url);
        }

        public static void EncerrarNavegador()
        {
            EncerrarWebDriver();
        }
    }
}
