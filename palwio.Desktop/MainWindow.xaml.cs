using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using palwio.Shared;

namespace palwio.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataLibrary lib = new ();
        public MainWindow()
        {
            lib.Read();
            lib.Drive();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            exerciseOutput.Text = lib.PickSolution(1); 
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            exerciseOutput.Text = lib.PickSolution(2); 
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            exerciseOutput.Text = lib.PickSolution(5); 
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            exerciseOutput.Text = lib.PickSolution(3); 
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            exerciseOutput.Text = lib.PickSolution(4);
        }

        private void P95Price_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Double.TryParse(P95Price.Text, out double parsedPrice))
            {
                lib.P95.price = parsedPrice;
            }
            else
            {
                lib.P95.price = lib.P95.savedPrice;
            }
        }

        private void LPGPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Double.TryParse(LPGPrice.Text, out double parsedPrice))
            {
                lib.LPG.price = parsedPrice;
            }
            else
            {
                lib.LPG.price = lib.LPG.savedPrice;
            }
        }

        private void InstallationPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Double.TryParse(InstallationPrice.Text, out double parsedPrice))
            {
                lib.installationCost = parsedPrice;
            }
            else
            {
                lib.installationCost = lib.savedInstallationCost;
            }
        }
    }
}
