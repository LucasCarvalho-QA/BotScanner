using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BotScanner._02___Utilidades
{
    public static class Dinheiro
    {
        public static string FormatarPreco(string preco)
        {
            var match = Regex.Match(preco, @"\d+,\d{2}");
            string precoFormatado = match.Value.Replace(".", "").Replace(",", ".");
            precoFormatado = precoFormatado.Remove(precoFormatado.Length - 1); ;

            return match.Success ? precoFormatado : "Valor inválido";
        }
    }
}
