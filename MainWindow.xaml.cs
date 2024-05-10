using BotScanner._00___Setup;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txtDataHoraAtual.Text = DateTime.Now.ToString();
            Thread.Sleep(1000);


            //Projeto projeto = new();            
            //projeto.IniciarNavegador();         
            //projeto.EncerrarNavegador();
            
        }

        public void CarregarLogotipos()
        {
            
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            cmbMarcas.SelectedIndex = -1;
        }

        private void PreencherComboBoxMarcas()
        {
            List<string> marcas = ["Guess Brasil", "Rovitex"];
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
    }
}