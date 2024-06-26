﻿using ClosedXML.Excel;
using Irony.Ast;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BotScanner._02___Utilidades.Relatorio
{
    public class PlanilhaPage
    {
        public string DataHora { get; set; }
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
        public string TamanhoEsperado { get; set; }
        public string TamanhoEncontrado { get; set; }

        public string Duracao { get; set; }
        public string Observacao { get; set; }
        public string LinkBusca { get; set; }
        public string LinkConectaLa { get; set; }


        public static List<PlanilhaPage> PlanilhaCarregada = new List<PlanilhaPage>();
        public static string diretorioPlanilha = string.Empty;
        public static bool produtoEncontrado;
        public static string diretorioDestinoPlanilha = string.Empty;
        public static DirectoryInfo TestResultFolder = Directory.CreateDirectory(@"04 - Evidencias");
        public static DirectoryInfo DatetimeFolder = Directory.CreateDirectory(Path.Combine(TestResultFolder.FullName, DateTime.Now.Date.ToString("dd-MM-yyyy")));

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
            var planilha = Path.Combine(diretorioDestinoPlanilha, nomeArquivo);
            diretorioPlanilha = planilha;

            return new XLWorkbook(planilha);
        }

        public static void SalvarLog_ProdutosValidados(string text, string filename )
        {
            FileStream fs = new FileStream(Path.Combine(diretorioDestinoPlanilha, filename), FileMode.Append, FileAccess.Write);
            using var sw = new StreamWriter(fs);
            sw.WriteLine($"{text}");            
        }

        public static List<string> RetornarLista_ProdutosValidados(string filename)
        {            
            List<string> produtosValidados = new List<string>();
         
            using (FileStream fs = new FileStream(Path.Combine(DatetimeFolder.FullName, filename), FileMode.OpenOrCreate, FileAccess.Read))
            {                
                using (StreamReader sr = new StreamReader(fs))
                {
                    string linha;
                    while ((linha = sr.ReadLine()) != null)
                    {                    
                        if (!string.IsNullOrEmpty(linha) && !linha.Trim().Equals(","))
                        {                     
                            produtosValidados.Add(linha);
                        }
                    }
                }
            }
            
            return produtosValidados;
        }

        public static void MoverArquivo(string nomeArquivo)
        {
            var diretorioAtual = Path.Combine(Environment.CurrentDirectory, nomeArquivo); 

            diretorioDestinoPlanilha = DatetimeFolder.FullName;

            File.Move(diretorioAtual, Path.Combine(DatetimeFolder.FullName, $"{nomeArquivo}"));
        }

        public static void CriarDiretorioEvidencias()
        {
            //Release
            TestResultFolder = Directory.CreateDirectory(@"04 - Evidencias");
            //TestResultFolder = Directory.CreateDirectory(@"..\..\..\04 - Evidencias");
            //PlanilhaPage.SalvarLog_ProdutosValidados("0", "Rovitex_Logs.txt");
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
                    DataHora = linha.Cell("A1").GetValue<string>(),
                    Seller = linha.Cell("B1").GetValue<string>(),
                    SKU_Parceiro = linha.Cell("C1").GetValue<string>(),
                    Status = linha.Cell("D1").GetValue<string>(),
                    ItemEncontrado = linha.Cell("E1").GetValue<string>(),

                    NomeEsperado = linha.Cell("F1").GetValue<string>(),
                    NomeEncontrado = linha.Cell("G1").GetValue<string>(),
                    DescricaoEsperada = linha.Cell("H1").GetValue<string>(),
                    DescricaoEncontrada = linha.Cell("I1").GetValue<string>(),
                    PrecoEsperado = linha.Cell("J1").GetValue<string>(),
                    PrecoEncontrado = linha.Cell("K1").GetValue<string>(),
                    CoresEsperadas = linha.Cell("L1").GetValue<string>(),
                    CoresEncontradas = linha.Cell("M1").GetValue<string>(),
                    TamanhoEsperado = linha.Cell("N1").GetValue<string>(),
                    TamanhoEncontrado = linha.Cell("O1").GetValue<string>(),

                    Observacao = linha.Cell("P1").GetValue<string>(),                    
                    Duracao = linha.Cell("Q1").GetValue<string>(),
                    LinkBusca = linha.Cell("R1").GetValue<string>(),
                    LinkConectaLa = linha.Cell("S1").GetValue<string>()
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
            worksheet.Cell("A1").Value = "Data/Hora";
            worksheet.Cell("B1").Value = "Seller";
            worksheet.Cell("C1").Value = "SKU Parceiro";
            worksheet.Cell("D1").Value = "Status";
            worksheet.Cell("E1").Value = "Item encontrado?";            

            worksheet.Cell("F1").Value = "Nome esperado";
            worksheet.Cell("G1").Value = "Nome encontrado";
            worksheet.Cell("H1").Value = "Descricao esperada";
            worksheet.Cell("I1").Value = "Descricao encontrada";
            worksheet.Cell("J1").Value = "Preco esperado";
            worksheet.Cell("K1").Value = "Preco encontrado";
            worksheet.Cell("L1").Value = "Cores esperadas";
            worksheet.Cell("M1").Value = "Cores encontradas";
            worksheet.Cell("N1").Value = "Tamanhos esperados";
            worksheet.Cell("O1").Value = "Tamanhos encontrados";

            worksheet.Cell("P1").Value = "Observação";
            worksheet.Cell("Q1").Value = "Duração";
            worksheet.Cell("R1").Value = "Link do produto";
            worksheet.Cell("S1").Value = "Link ConectaLa";


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

            worksheet.Cell($"A{linhaVazia}").Value = novosDados.DataHora;
            worksheet.Cell($"B{linhaVazia}").Value = novosDados.Seller;
            worksheet.Cell($"C{linhaVazia}").Value = novosDados.SKU_Parceiro;
            worksheet.Cell($"D{linhaVazia}").Value = novosDados.Status == "True" ? "Sucesso" : "Item divergente";
            worksheet.Cell($"D{linhaVazia}").Style.Fill.BackgroundColor = novosDados.Status == "True" ? XLColor.Green : XLColor.Red;
            
            worksheet.Cell($"E{linhaVazia}").Value = produtoEncontrado ? "Sim" : "Não";
            worksheet.Cell($"E{linhaVazia}").Style.Fill.BackgroundColor = produtoEncontrado ? XLColor.Green : XLColor.Red;

            worksheet.Cell($"F{linhaVazia}").Value = novosDados.NomeEsperado;
            worksheet.Cell($"F{linhaVazia}").Style.Fill.BackgroundColor = XLColor.GreenYellow;
            worksheet.Cell($"G{linhaVazia}").Value = novosDados.NomeEncontrado;
            worksheet.Cell($"G{linhaVazia}").Style.Fill.BackgroundColor = RetornarEstilo(novosDados.NomeEsperado, novosDados.NomeEncontrado);

            worksheet.Cell($"H{linhaVazia}").Value = novosDados.DescricaoEsperada;
            worksheet.Cell($"H{linhaVazia}").Style.Fill.BackgroundColor = XLColor.GreenYellow;
            worksheet.Cell($"I{linhaVazia}").Value = novosDados.DescricaoEncontrada;
            worksheet.Cell($"I{linhaVazia}").Style.Fill.BackgroundColor = RetornarEstilo(novosDados.DescricaoEsperada, novosDados.DescricaoEncontrada);

            worksheet.Cell($"J{linhaVazia}").Value = novosDados.PrecoEsperado;
            worksheet.Cell($"J{linhaVazia}").Style.Fill.BackgroundColor = XLColor.GreenYellow;
            worksheet.Cell($"K{linhaVazia}").Value = novosDados.PrecoEncontrado;
            worksheet.Cell($"K{linhaVazia}").Style.Fill.BackgroundColor = RetornarEstilo(novosDados.PrecoEsperado, novosDados.PrecoEncontrado);

            worksheet.Cell($"L{linhaVazia}").Value = novosDados.CoresEsperadas;
            worksheet.Cell($"L{linhaVazia}").Style.Fill.BackgroundColor = XLColor.GreenYellow;
            worksheet.Cell($"M{linhaVazia}").Value = novosDados.CoresEncontradas;
            worksheet.Cell($"M{linhaVazia}").Style.Fill.BackgroundColor = RetornarEstilo(novosDados.CoresEsperadas, novosDados.CoresEncontradas);

            worksheet.Cell($"N{linhaVazia}").Value = novosDados.TamanhoEsperado;
            worksheet.Cell($"N{linhaVazia}").Style.Fill.BackgroundColor = XLColor.GreenYellow;
            worksheet.Cell($"O{linhaVazia}").Value = novosDados.TamanhoEncontrado;
            worksheet.Cell($"O{linhaVazia}").Style.Fill.BackgroundColor = RetornarEstilo(novosDados.TamanhoEsperado, novosDados.TamanhoEncontrado);
            
            worksheet.Cell($"P{linhaVazia}").Value = novosDados.Observacao;
            worksheet.Cell($"Q{linhaVazia}").Value = novosDados.Duracao;
            worksheet.Cell($"R{linhaVazia}").Value = novosDados.LinkBusca;
            worksheet.Cell($"S{linhaVazia}").Value = novosDados.LinkConectaLa;

            workbook.Save();
        }

        public static XLColor RetornarEstilo(string resultadoEsperado, string resultadoObtido)
        {
            return resultadoEsperado.Equals(resultadoObtido) && produtoEncontrado.Equals(true) ? XLColor.Green : XLColor.Red;
        }

    }
}

