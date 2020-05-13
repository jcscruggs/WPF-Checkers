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
using System.Windows.Shapes;

namespace checkers_game
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        public void start_game(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(); // make a new main window obj
            this.Visibility = Visibility.Collapsed; // hide current window
            mainWindow.Show(); // show the main window obj
            this.Close(); // terminate current window
            


        }

        public void options(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
