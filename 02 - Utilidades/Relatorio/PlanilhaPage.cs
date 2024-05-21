using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotScanner._02___Utilidades.Relatorio
{
    public class PlanilhaPage
    {
        public string Seller { get; set; }                
        public string SKU_Parceiro { get; set; }
        public string Status { get; set; } //bool
        public string ItemEncontrado { get; set; } //bool


        public string NomeEsperado { get; set; }
        public string NomeEncontrado { get; set; }
        public string DescricaoEsperada { get; set; }
        public string DescricaoEncontrada { get; set; }
        public string PrecoEsperado { get; set; }
        public string PrecoEncontrado { get; set; }
        public string CoresEsperadas { get; set; }
        public string CoresEncontradas { get; set; }


        public string Duracao { get; set; }
        public string Observacao { get; set; }
        public string LinkBusca { get; set; }
        public string LinkConectaLa { get; set; }


        public static List<PlanilhaPage> PlanilhaCarregada = new List<PlanilhaPage>();
        public static string diretorioPlanilha = string.Empty;

        public List<PlanilhaPage> CarregarDadosPlanilha_PorSeller(string seller)
        {
            return FiltrarPorSeller(PlanilhaCarregada, seller);
        }

        public static List<PlanilhaPage> FiltrarPorSeller(List<PlanilhaPage> planilhaPrivalia, string sellerFiltrado)
        {
            return planilhaPrivalia.Where(item => item.Seller.Equals(sellerFiltrado, StringComparison.OrdinalIgnoreCase)).Take(50).ToList();
        }

        public static XLWorkbook CarregarPlanilha(string nomeArquivo)
        {
            string nomeArquivoFormatado = $@"05 - Dados\\{nomeArquivo}.xlsx".Replace(".xlsx.xlsx", ".xlsx");

            var diretorioAtual = Directory.GetCurrentDirectory();
            var planilha = Path.Combine(diretorioAtual, nomeArquivoFormatado);
            diretorioPlanilha = planilha;

            return new XLWorkbook(planilha);
        }

        public static void MoverArquivo(string nomeArquivo)
        {
            var diretorioAtual = Path.Combine(Environment.CurrentDirectory, nomeArquivo);
            var diretorioDestino = Path.Combine(Environment.CurrentDirectory, $@"05 - Dados\\{nomeArquivo}").Replace("\\\\","\\");

            File.Move(diretorioAtual, diretorioDestino);
        }

        public static List<PlanilhaPage> CarregarDadosDaPlanilha(string nomeArquivo)
        {
            var workbook = CarregarPlanilha(nomeArquivo);
            var worksheet = workbook.Worksheet(1);
            var dados = new List<PlanilhaPage>();

            var linhas = worksheet.RangeUsed().RowsUsed().Skip(1);

            foreach (var linha in linhas)
            {
              var dado = new PlanilhaPage
              {
                    Seller = linha.Cell("A1").GetValue<string>(),
                    SKU_Parceiro = linha.Cell("B1").GetValue<string>(),
                    Status = linha.Cell("C1").GetValue<string>(),
                    ItemEncontrado = linha.Cell("D1").GetValue<string>(),

                    NomeEsperado = linha.Cell("E1").GetValue<string>(),
                    NomeEncontrado = linha.Cell("F1").GetValue<string>(),
                    DescricaoEsperada = linha.Cell("G1").GetValue<string>(),
                    DescricaoEncontrada = linha.Cell("H1").GetValue<string>(),
                    PrecoEsperado = linha.Cell("I1").GetValue<string>(),
                    PrecoEncontrado = linha.Cell("J1").GetValue<string>(),
                    CoresEsperadas = linha.Cell("K1").GetValue<string>(),
                    CoresEncontradas = linha.Cell("L1").GetValue<string>(),

                    Observacao = linha.Cell("M1").GetValue<string>(),                    
                    Duracao = linha.Cell("N1").GetValue<string>(),
                    LinkBusca = linha.Cell("O1").GetValue<string>(),
                    LinkConectaLa = linha.Cell("P1").GetValue<string>()
              };

                dados.Add(dado);
            }

            return dados;
        }

        public static string GerarPlanilha(string seller)
        {
            using var workbook = new XLWorkbook();
            string localRunId = $"Local_Run_{DateTime.Now:HHmmss}";

            var worksheet = workbook.Worksheets.Add("BotScanner");
            worksheet.Cell("A1").Value = "Seller";
            worksheet.Cell("B1").Value = "SKU Parceiro";
            worksheet.Cell("C1").Value = "Status";
            worksheet.Cell("D1").Value = "Item encontrado?";            

            worksheet.Cell("E1").Value = "Nome esperado";
            worksheet.Cell("F1").Value = "Nome encontrado";
            worksheet.Cell("G1").Value = "Descricao esperada";
            worksheet.Cell("H1").Value = "Descricao encontrada";
            worksheet.Cell("I1").Value = "Preco esperado";
            worksheet.Cell("J1").Value = "Preco encontrado";
            worksheet.Cell("K1").Value = "Cores esperadas";
            worksheet.Cell("L1").Value = "Cores encontradas";
            
            worksheet.Cell("M1").Value = "Observação";
            worksheet.Cell("N1").Value = "Duração";
            worksheet.Cell("O1").Value = "Link do produto";
            worksheet.Cell("P1").Value = "Link ConectaLa";


            string nomeDoArquivo = $"{seller} - {localRunId}.xlsx";
            workbook.SaveAs(nomeDoArquivo);

            MoverArquivo(nomeDoArquivo);

            return nomeDoArquivo;
        }

        public static void AtualizarPlanilha(string nomeArquivo, PlanilhaPage novosDados)
        {
            var workbook = CarregarPlanilha(nomeArquivo);
            var worksheet = workbook.Worksheet(1);

            int linhaVazia = worksheet.LastRowUsed().RowNumber() + 1;

            worksheet.Cell($"A{linhaVazia}").Value = novosDados.Seller;
            worksheet.Cell($"B{linhaVazia}").Value = novosDados.SKU_Parceiro;
            worksheet.Cell($"C{linhaVazia}").Value = novosDados.Status == "True" ? "Sucesso" : "Item divergente";
            worksheet.Cell($"C{linhaVazia}").Style.Fill.BackgroundColor = novosDados.Status == "True" ? XLColor.Green : XLColor.Red;
            worksheet.Cell($"D{linhaVazia}").Value = novosDados.ItemEncontrado;

            worksheet.Cell($"E{linhaVazia}").Value = novosDados.NomeEsperado;
            worksheet.Cell($"E{linhaVazia}").Style.Fill.BackgroundColor = XLColor.GreenYellow;
            worksheet.Cell($"F{linhaVazia}").Value = novosDados.NomeEncontrado;
            worksheet.Cell($"F{linhaVazia}").Style.Fill.BackgroundColor = RetornarEstilo(novosDados.NomeEsperado, novosDados.NomeEncontrado);

            worksheet.Cell($"G{linhaVazia}").Value = novosDados.DescricaoEsperada;
            worksheet.Cell($"G{linhaVazia}").Style.Fill.BackgroundColor = XLColor.GreenYellow;
            worksheet.Cell($"H{linhaVazia}").Value = novosDados.DescricaoEncontrada;
            worksheet.Cell($"H{linhaVazia}").Style.Fill.BackgroundColor = RetornarEstilo(novosDados.DescricaoEsperada, novosDados.DescricaoEncontrada);

            worksheet.Cell($"I{linhaVazia}").Value = novosDados.PrecoEsperado;
            worksheet.Cell($"I{linhaVazia}").Style.Fill.BackgroundColor = XLColor.GreenYellow;
            worksheet.Cell($"J{linhaVazia}").Value = novosDados.PrecoEncontrado;
            worksheet.Cell($"J{linhaVazia}").Style.Fill.BackgroundColor = RetornarEstilo(novosDados.PrecoEsperado, novosDados.PrecoEncontrado);

            worksheet.Cell($"K{linhaVazia}").Value = novosDados.CoresEsperadas;
            //worksheet.Cell($"K{linhaVazia}").Style.Fill.BackgroundColor = XLColor.GreenYellow;
            worksheet.Cell($"L{linhaVazia}").Value = novosDados.CoresEncontradas;
            //worksheet.Cell($"L{linhaVazia}").Style.Fill.BackgroundColor = RetornarEstilo(novosDados.CoresEsperadas, novosDados.CoresEncontradas);

            worksheet.Cell($"M{linhaVazia}").Value = novosDados.Duracao;
            worksheet.Cell($"N{linhaVazia}").Value = novosDados.Observacao;
            worksheet.Cell($"O{linhaVazia}").Value = novosDados.LinkBusca;
            worksheet.Cell($"P{linhaVazia}").Value = novosDados.LinkConectaLa;

            workbook.Save();
        }

        public static XLColor RetornarEstilo(string resultadoEsperado, string resultadoObtido)
        {
            return resultadoEsperado.Equals(resultadoObtido) ? XLColor.Green : XLColor.Red;
        }

    }
}

