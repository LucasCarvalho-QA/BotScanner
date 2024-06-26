﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            ChromeOptions chromeOptions = new();
            chromeOptions.AddArguments("--start-maximized", "ignore-certificate-errors");

            if (parametros.Headless)
                chromeOptions.AddArgument("--headless=new");

            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;  // Suprimir a janela do console

            driver = new ChromeDriver(service, chromeOptions);
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

        public static IWebElement BuscarElemento(By buscarPor)
        {
            try
            {
                return wait.Until(ExpectedConditions.ElementToBeClickable(buscarPor));
            }
            catch (Exception)
            {
               

                return null;
            }
        }

        public static ReadOnlyCollection<IWebElement> BuscarListaElementos(By buscarPor)
        {
            try
            {
                return wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(buscarPor));
            }
            catch (Exception)
            {


                return null;
            }
        }

        public static bool VerificarVisibilidadeDoElemento(By buscarPor)
        {
            try
            {
                return wait.Until(ExpectedConditions.ElementIsVisible(buscarPor)).Displayed;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static IWebElement BuscarElemento_PorXpath(string xpath)
        {
            return BuscarElemento(By.XPath(xpath));
        }

        public static IWebElement BuscarElemento_PorCssSelector(string cssSelector)
        {
            return BuscarElemento(By.CssSelector(cssSelector));
        }

        public static IWebElement BuscarElemento_PorClassName(string className)
        {
            return BuscarElemento(By.ClassName(className));
        }

        public static ReadOnlyCollection<IWebElement> BuscarListaDeElementos_PorCssSelector(string cssSelector)
        {
            return BuscarListaElementos(By.CssSelector(cssSelector));
        }

        public static ReadOnlyCollection<IWebElement> BuscarListaDeElementos_PorClassName(string className)
        {
            return BuscarListaElementos(By.ClassName(className));
        }

        public static IWebElement BuscarElemento_PorID(string id)
        {
            return BuscarElemento(By.Id(id));
        }


        public static string BuscarTextoDoElementoPorID(string id)
        {
            return BuscarElemento_PorID(id).Text;
        }

        public static string BuscarTextoDoElemento_PorXpath(string xpath)
        {
            return BuscarElemento_PorXpath(xpath).Text;
        }

        public static void RealizarClique_PorXpath(string xpath)
        {
            BuscarElemento_PorXpath(xpath).Click();
        }

        public static void RealizarClique_PorID(string id)
        {
            BuscarElemento_PorID(id).Click();
        }

        public static void RealizarCliqueSimulandoUsuario(By buscarPor)
        {
            var elemento = BuscarElemento(buscarPor);

            Actions builder = new Actions(driver);
            builder
                .MoveToElement(elemento)
                .Click()
                .Build()
                .Perform();
        }
    }
}
