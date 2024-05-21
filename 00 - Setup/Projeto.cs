using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotScanner._00___Setup
{
    
    public class Projeto : WebDriver
    {
        
        public static void IniciarNavegador()
        {
            ParametrosWebDriver parametros = new() 
            { 
                Headless = true, 
                TempoEspera = 5 
            };

            ConfigurarWebDriverChrome(parametros);            
        }

        public static void EncerrarNavegador()
        {
            EncerrarWebDriver();
        }
    }
}
