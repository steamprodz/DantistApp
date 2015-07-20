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

namespace DantistApp
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private StackPanel ThreatmentPlanPanel;

        public SettingsWindow(StackPanel _stackPanel)
        {
            InitializeComponent();

            ThreatmentPlanPanel = _stackPanel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

            for (int i = 0; i < ThreatmentPlanPanel.Children.Count - 1; i++)
            {
                var comment = (ThreatmentPlanPanel.Children[i] as UserControls.ReportElement).Comment;

                TextBox textBoxComment = new TextBox();
                textBoxComment.Text = comment;

                grid_comments.RowDefinitions.Add(new RowDefinition());
                grid_comments.Children.Add(textBoxComment);

                //var lll = grid_comments.RowDefinitions[1];
            }

            
        }
    }
}
