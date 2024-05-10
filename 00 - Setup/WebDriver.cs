using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotScanner._00___Setup
{
    public class ParametrosWebDriver
    {
        public bool Headless { get; set; }
        public int TempoEspera { get; set; }
    }

    public class WebDriver
    {
        public static ChromeDriver ?driver;
        public static WebDriverWait ?wait;
        public static string localRunId = string.Empty;

        public static void ConfigurarWebDriverChrome(ParametrosWebDriver parametros)
        {
            ChromeOptions? chromeOptions = new();
            chromeOptions.AddArguments("--start-maximized");
            chromeOptions.AddArguments("ignore-certificate-errors");                        

            if (parametros.Headless)
                chromeOptions.AddArgument("--headless=new");

            driver = new ChromeDriver(chromeOptions);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(parametros.TempoEspera));
        }

        public static void EncerrarWebDriver() 
        {
            driver.Quit();            
        }


        public static void NavegarPara(string url)
        {
            driver.Navigate().GoToUrl(url);
        }
    }
}
