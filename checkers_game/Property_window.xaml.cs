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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private Brush p1_color;
        private Brush p2_color;
        private int current_theme_index;
        
        public Window2()
        {
            InitializeComponent();
            p1_color = Brushes.Gold;
            p2_color = Brushes.Violet;
            current_theme_index = 0;

            Combobox_theme.SelectedIndex = 0; // set preset index to 0
        }

        private void return_home(object sender, RoutedEventArgs e)
        {
            if(current_theme_index == 1)
            {
                p1_color = Brushes.Gold;
                p2_color = Brushes.Silver;

            }else if (current_theme_index == 2)
            {
                p1_color = Brushes.Red;
                p2_color = Brushes.DarkGray;
            }
            else if (current_theme_index == 3)
            {
                p1_color = Brushes.Coral;
                p2_color = Brushes.LightSeaGreen;
            }

            if (current_theme_index == 0)
            {
                // if the theme was not changed from preset then call base constructor
                Window1 window1 = new Window1();
                this.Visibility = Visibility.Collapsed; // hide current window
                window1.Show(); // show the home window obj
                this.Close();
            }
            else
            {
                // if theme was changed then call overloaded constructor with new colors

                Window1 window1 = new Window1(this.p1_color, this.p2_color);// make a new home window obj
                this.Visibility = Visibility.Collapsed; // hide current window
                window1.Show(); // show the home window obj
                this.Close();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender; // get combobox object from sender

            current_theme_index = comboBox.SelectedIndex; // get index of chosen combobox item
        }
    }
}

    
