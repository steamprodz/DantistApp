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

            foreach (var item in grid_nij_chel.Children)
            {
                if (item is UnlimitedElement)
                {
                    UnlimitedElement element = item as UnlimitedElement;
                    element.MouseLeftButtonDown += UnlimitedElement_Adding;
                }
            }
            foreach (var item in grid_ver_chel.Children)
            {
                if (item is GroupElement)
                {
                    GroupElement element = item as GroupElement;
                    element.MouseLeftButtonDown += GroupElement_Adding;
                }
            }
        }


    }
}
