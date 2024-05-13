using BotScanner._00___Setup;
using BotScanner._01___Sellers;
using BotScanner._02___Utilidades;
using BotScanner._02___Utilidades.ConectaLa;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BotScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PreencherComboBoxMarcas();  
        }

        public static Produtos produtosSelecionados = null;
        public static int itensValidados = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {   
            if (cmbMarcas.SelectedIndex == -1 && chkSelecionarTodasMarcas.IsChecked == false)
                MessageBox.Show("Selecione ao menos uma marca/seller para continuar");
            else
                IniciarProjeto(cmbMarcas.SelectedItem.ToString());
        }

        public void AtribuirQuantidadeItens()
        {
            txtQuantidadeTotalItens.Text = produtosSelecionados.result.registers_count.ToString();
        }

        public void AtribuirItensRestantes()
        {
            txtItensPendentesValidacao.Text = (produtosSelecionados.result.registers_count - itensValidados).ToString();
        }

        public void AtribuirItensValidados()
        {
            txtItensValidados.Text = itensValidados.ToString();
        }

        public void IniciarProjeto(string seller)
        {
            txtDataHoraAtual.Text = DateTime.Now.ToString();

            produtosSelecionados = Produtos.SelecionarProdutos();

            AtribuirQuantidadeItens();
            AtribuirItensRestantes();
            AtribuirItensValidados();            

            AcessarFluxoSeller(seller);                        
        }


        public void AcessarFluxoSeller(string seller)
        {            
            switch (seller)
            {
                case "Rovitex":                    
                    Rovitex.FluxoRovitex();
                    break;
                default:            
                    break;
            }            
        }



        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            cmbMarcas.SelectedIndex = -1;
        }

        private void PreencherComboBoxMarcas()
        {
            List<string> marcas = ["Rovitex"];
            cmbMarcas.ItemsSource = marcas.OrderBy(marca => marca).ToList();
        }

        private void cmbMarcas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (chkSelecionarTodasMarcas.IsChecked == true)            
                chkSelecionarTodasMarcas.IsChecked = false;
            

            if (cmbMarcas.SelectedItem != null)
            {
                string selectedBrand = cmbMarcas.SelectedItem.ToString();
                MessageBox.Show("Marca selecionada: " + selectedBrand);
            }
        }

        private void txtLog_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}