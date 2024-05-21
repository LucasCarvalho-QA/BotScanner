using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotScanner._02___Utilidades
{
    public static class StringFormatter
    {
        public static string EliminarQuebrasDeLinha(string texto)
        {
            return texto.Replace("\n", " ").Replace("\\n"," ");
        }

        public static string EliminarEspacoDuplo(string texto)
        {
            return texto.Replace("  ", " ").Replace("   "," ");
        }


        public static string FormatarTexto_Descricao(string texto)
        {
            texto = EliminarQuebrasDeLinha(texto);
            texto = EliminarEspacoDuplo(texto);

            return texto;
        }


    }
}
