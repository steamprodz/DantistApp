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
using DantistApp.Elements;
using System.Text.RegularExpressions;

namespace DantistApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Element _activeElement;
        Point _mousePosition;

        public MainWindow()
        {
            InitializeComponent();

            foreach (var item in panel_btns.Children)
            {
                if (item is Button)
                    (item as Button).Click += PanelBtn_Click;
            }

            foreach (var item in grid_nij_chel.Children)
            {
                if (item is UnlimitedElement)
                    (item as UnlimitedElement).MouseLeftButtonDown += UnlimitedElement_Adding;
            }
            foreach (var item in grid_ver_chel.Children)
            {
                if (item is GroupElement)
                    (item as GroupElement).MouseLeftButtonDown += GroupElement_Adding;
            }
        }

        private void PanelBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            String subStr = btn.Content.ToString().Substring(0, 3);
            int number = Convert.ToInt32(Regex.Match(subStr, @"\d+").Value);

            tabControl_elements.SelectedIndex = number - 1;
        }


    }
}
