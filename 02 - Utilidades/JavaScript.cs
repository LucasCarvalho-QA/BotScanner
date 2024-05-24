using BotScanner._00___Setup;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Math;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotScanner._02___Utilidades
{
    public class JavaScript : Projeto
    {
        public static object BuscarTextoEmElemento(string script, string elementoJS)
        {            
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector(elementoJS)));

            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
            var resultado = executor.ExecuteScript(script);

            return resultado;
        }
    }
}
