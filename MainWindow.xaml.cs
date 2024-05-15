using BotScanner._00___Setup;
using BotScanner._01___Sellers;
using BotScanner._02___Utilidades;
using BotScanner._02___Utilidades.ConectaLa;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
using System.Xml.Linq;

namespace BotScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public class MainViewModel : INotifyPropertyChanged
    {
        private string _logText;
        public string LogText
        {
            get { return _logText; }
            set
            {
                if (_logText != value)
                {
                    _logText = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            DataContext = ViewModel;

            PreencherComboBoxMarcas();
        }

        public static Produtos produtosSelecionados = null;
        public static int itensValidados = 0;
        public static int quantidadeTotalItens = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMarcas.SelectedIndex == -1 && chkSelecionarTodasMarcas.IsChecked == false)
                MessageBox.Show("Selecione ao menos uma marca/seller para continuar");
            else
                IniciarProjeto(cmbMarcas.SelectedItem.ToString());
        }

        public void AtribuirQuantidadeItens()
        {
            txtQuantidadeTotalItens.Text = quantidadeTotalItens.ToString();
        }

        public void AtribuirItensRestantes()
        {
            txtItensPendentesValidacao.Text = (quantidadeTotalItens - itensValidados).ToString();
        }

        public void AtribuirItensValidados()
        {
            txtItensValidados.Text = itensValidados.ToString();
        }

        public void PopularVariaveisIniciais()
        {
            quantidadeTotalItens = produtosSelecionados.result.registers_count;

            AtribuirQuantidadeItens();
            AtribuirItensRestantes();
            AtribuirItensValidados();
        }

        public void IniciarProjeto(string seller)
        {
            txtDataHoraAtual.Text = DateTime.Now.ToString();

            AcessarFluxoSeller(seller);
        }

        public void AcessarFluxoSeller(string seller)
        {
            switch (seller)
            {
                case "Rovitex":
                    Rovitex rovitex = new();
                    Rovitex.FluxoRovitex();
                    PopularVariaveisIniciais();
                    break;
                default:
                    break;
            }
        }

        public void AtualizarLog(string name)
        {
            ViewModel.LogText = $"Validando o item '{name}'";
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            cmbMarcas.SelectedIndex = -1;
        }

        private void PreencherComboBoxMarcas()
        {
            List<string> marcas = new() { "Rovitex" };
            cmbMarcas.ItemsSource = marcas.OrderBy(marca => marca).ToList();
        }

        private void cmbMarcas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (chkSelecionarTodasMarcas.IsChecked == true)
                chkSelecionarTodasMarcas.IsChecked = false;

            if (cmbMarcas.SelectedItem != null)
            {
                string selectedBrand = cmbMarcas.SelectedItem.ToString();
                //MessageBox.Show("Marca selecionada: " + selectedBrand);
            }
        }

        private void txtLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.LogText = $"Validando o item ''";
        }
    }
}