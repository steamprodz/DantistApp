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
        PanoramaWindow _panoramaWindow;

        public MainWindow()
        {
            InitializeComponent();

            foreach (var item in panel_btns.Children)
            {
                if (item is Button)
                    (item as Button).Click += PanelBtn_Click;
            }

            AddDoubleClickEvents();
        }

        /// <summary>
        /// Adds double-click events for basic elements
        /// </summary>
        private void AddDoubleClickEvents()
        {
            List<Grid> grids = new List<Grid>();
            foreach (TabItem tabItem in tabControl_elements.Items)
            {
                Grid grid = null;
                if (tabItem.Content is Grid)
                    grid = tabItem.Content as Grid;
             
                if (grid != null)
                    foreach (var item in grid.Children)
                    {
                        try
                        {
                            (item as Element).MouseLeftButtonDown += Element_AddingOnDoubleClick;
                        }
                        catch { }
                    }
            }
        }

        /// <summary>
        /// Click from right side panel (with text descriptions)
        /// calls bottom tabItem selecting
        /// </summary>
        private void PanelBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            String subStr = btn.Content.ToString().Substring(0, 3);
            int number = Convert.ToInt32(Regex.Match(subStr, @"\d+").Value);

            tabControl_elements.SelectedIndex = number - 1;
        }

        private void MenuItem_Service_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow wnd = new SettingsWindow();
            wnd.ShowDialog();
        }

        private void MenuItem_PanoramaWindow_Click(object sender, RoutedEventArgs e)
        {
            if (_panoramaWindow == null || !_panoramaWindow.IsLoaded)
            {
                _panoramaWindow = new PanoramaWindow();
                _panoramaWindow.Show();
            }
            else
                _panoramaWindow.Activate();
        }

        private void MenuItem_DeleteAllElements_Click(object sender, RoutedEventArgs e)
        {
            int n = canvas_main.Children.Count;
            for (int i = n-1; i >= 0; i--)
            {
                if (canvas_main.Children[i] is IControlManipulate)
                {
                    canvas_main.Children.RemoveAt(i);
                }
            }
        }



    }



}
