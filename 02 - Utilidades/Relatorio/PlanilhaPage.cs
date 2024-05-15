using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotScanner._02___Utilidades.Relatorio
{
    internal class PlanilhaPage
    {
        public string Seller { get; set; }                
        public string SKU_Parceiro { get; set; }
        public bool Status { get; set; }
        public bool ItemEncontrado { get; set; }


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
        

        public static List<PlanilhaPage> PlanilhaCarregada = new List<PlanilhaPage>();

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

            return new XLWorkbook(planilha);
        }

        public static void MoverArquivo(string nomeArquivo)
        {
            var diretorioAtual = Path.Combine(Environment.CurrentDirectory, nomeArquivo);
            var diretorioDestino = Path.Combine(Environment.CurrentDirectory, $@"05 - Dados\\{nomeArquivo}");

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
                    Seller = linha.Cell("A1").Value = "Seller",
                    SKU_Parceiro = linha.Cell("B1").Value = "SKU Parceiro",
                    Status = linha.Cell("C1").Value = "Status",
                    ItemEncontrado = linha.Cell("D1").Value = "Item encontrado?",

                    NomeEsperado = linha.Cell("E1").Value = "Nome esperado",
                    NomeEncontrado = linha.Cell("F1").Value = "Nome encontrado",
                    DescricaoEsperada = linha.Cell("G1").Value = "Descricao esperada",
                    DescricaoEncontrada = linha.Cell("H1").Value = "Descricao encontrada",
                    PrecoEsperado = linha.Cell("I1").Value = "Preco esperado",
                    PrecoEncontrado = linha.Cell("J1").Value = "Preco encontrado",
                    CoresEsperadas = linha.Cell("K1").Value = "Cores esperadas",
                    CoresEncontradas = linha.Cell("L1").Value = "Cores encontradas",

                    Observacao = linha.Cell("M1").Value = "Observação",
                    linha.Cell("N1").Value = "Link do produto",

                    linha.Cell("O1").Value = "Duração",
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
            worksheet.Cell("N1").Value = "Link do produto";
            worksheet.Cell("O1").Value = "Duração";

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

            worksheet.Cell("A" + linhaVazia).Value = novosDados.Seller;
            worksheet.Cell("B" + linhaVazia).Value = novosDados.SKU_Parceiro;
            worksheet.Cell("C" + linhaVazia).Value = novosDados.NomeItem;
            worksheet.Cell("D" + linhaVazia).Value = novosDados.Status;
            worksheet.Cell("D" + linhaVazia).Style.Fill.BackgroundColor = novosDados.Status ? XLColor.Green : XLColor.Red;
            worksheet.Cell("D" + linhaVazia).Style.Font.FontColor = XLColor.White;

            worksheet.Cell("E" + linhaVazia).Value = novosDados.ObservacaoProduto;
            worksheet.Cell("F" + linhaVazia).Value = novosDados.ObservacaoDescricao;
            worksheet.Cell("G" + linhaVazia).Value = novosDados.ObservacoesGerais;

            if (!string.IsNullOrEmpty(novosDados.ObservacaoDescricao) && novosDados.Status == false)
            {
                worksheet.Cell("D" + linhaVazia).Value = "Produto inconsistente";
                worksheet.Cell("D" + linhaVazia).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Cell("D" + linhaVazia).Style.Font.FontColor = XLColor.Black;
                worksheet.Cell("G" + linhaVazia).Value = "Necessita análise manual";
            }

            worksheet.Cell("G" + linhaVazia).Value = UrlPesquisa;
            workbook.Save();
        }

    }
}
}
