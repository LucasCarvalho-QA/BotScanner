using BotScanner._00___Setup;
using BotScanner._01___Sellers;
using BotScanner._02___Utilidades;
using BotScanner._02___Utilidades.ConectaLa;
using DocumentFormat.OpenXml.Drawing;
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
    /// <summary>s
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public class MainViewModel : INotifyPropertyChanged
    {
        private string _logText;
        public string LogText
        {
            get => _logText;
            set
            {
                if (_logText != value)
                {
                    _logText = value;
                    OnPropertyChanged(nameof(LogText));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; set; }
        private DateTime _startTime;
        private DateTime _endTime;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            DataContext = ViewModel;

            PreencherComboBoxMarcas();
        }

        public static Produtos produtosSelecionados = null;
        public static int itensValidados = 0;
        public static int itensValidadosOK = 1;
        public static int itensValidadosNOK = 1;
        public static int quantidadeTotalItens = 0;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMarcas.SelectedIndex == -1 && chkSelecionarTodasMarcas.IsChecked == false)
                MessageBox.Show("Selecione ao menos uma marca/seller para continuar");
            else
                await IniciarProjeto(cmbMarcas.SelectedItem.ToString());
        }

        public async Task IniciarProjeto(string seller)
        {
            _startTime = DateTime.Now;
            txtDataHoraAtual.Text = _startTime.ToString();            

            await AcessarFluxoSeller(seller);
        }

        public async Task CalcularPrevisaoTermino()
        {
            // Determine o número total de itens a serem processados
            int totalItems = produtosSelecionados.result.data.Count;

            // Estime o tempo total necessário (10 segundos por item)
            TimeSpan estimatedTime = TimeSpan.FromSeconds(totalItems * 10);

            // Calcule a previsão de término
            _endTime = _startTime.Add(estimatedTime);
            txtPrevisaoTermino.Text = _endTime.ToString();
        }

        public async Task FinalizarProjeto()
        {
            _endTime = DateTime.Now;            
            TimeSpan duration = _endTime - _startTime;
            txtTempoDecorrido.Text = duration.ToString(@"hh\:mm\:ss");

        }

        public async Task AcessarFluxoSeller(string seller)
        {
            switch (seller)
            {
                case "Rovitex":
                    Rovitex rovitex = new();
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    await rovitex.CarregarProdutosAsync(); // Chamada assíncrona para carregar produtos
                    await AtribuirQuantidadeItens();
                    await PopularVariaveisIniciais(); // Chamada assíncrona
                    await CalcularPrevisaoTermino();
                    await Rovitex.FluxoRovitex(mainWindow);
                    
                    break;
                default:
                    break;
            }
            await FinalizarProjeto();
        }


        public async Task AtribuirQuantidadeItens()
        {          

            await Dispatcher.InvokeAsync(() =>
            {
                txtQuantidadeTotalItens.Text = quantidadeTotalItens.ToString();
            });
        }


        public async Task AtribuirItensRestantes()
        {            
            await Dispatcher.InvokeAsync(() =>
            {
                txtItensPendentesValidacao.Text = quantidadeTotalItens--.ToString();
            });
        }

        public async Task AtribuirItensValidados()
        {
            await Dispatcher.InvokeAsync(() =>
            {
                txtItensValidados.Text = itensValidados++.ToString();
            });
        }

        public async Task AtribuirItensOK()
        {
            await Dispatcher.InvokeAsync(() =>
            {
                txtItensValidadosOK.Text = itensValidadosOK++.ToString();
            });
        }

        public async Task AtribuirItensNotOK()
        {
            await Dispatcher.InvokeAsync(() =>
            {
                txtItensDivergentes.Text = itensValidadosNOK++.ToString();
            });
        }

        public async Task PopularVariaveisIniciais()
        {            
            await AtribuirItensRestantes();
            await AtribuirItensValidados();
        }




        public async Task AtualizarLogAsync(string name, string status)
        {
            status = status == "True" ? "OK" : "NOK";
            Dispatcher.Invoke(() =>
            {
                ViewModel.LogText += $"'{name}': {status}\n"; // Adiciona uma nova linha para cada item validado
            });
                        
            await AtribuirItensValidados();
            await AtribuirItensRestantes();

            if (status == "OK")
                await AtribuirItensOK();
            else
                await AtribuirItensNotOK();                        
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

        private void btnBaixarLog_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}