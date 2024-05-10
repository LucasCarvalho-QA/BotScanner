using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotScanner._00___Setup
{
    
    public class Projeto : WebDriver
    {
        
        public void IniciarNavegador()
        {
            ParametrosWebDriver parametros = new() 
            { 
                Headless = false, 
                TempoEspera = 5 
            };

            ConfigurarWebDriverChrome(parametros);
            NavegarPara("https://www.guessbrasil.com.br/");
        }

        public void EncerrarNavegador()
        {
            EncerrarWebDriver();
        }
    }
}
