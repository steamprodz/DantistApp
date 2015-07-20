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

namespace DantistApp.UserControls
{
    /// <summary>
    /// Interaction logic for ReportElement.xaml
    /// </summary>
    public partial class ReportElement : UserControl
    {
        public ReportElement()
        {
            InitializeComponent();
        }

        public string Comment { get; set; }

        public Canvas MainCanvas
        {
            get { return (Canvas)base.GetValue(MainCanvasProperty); }
            set { base.SetValue(MainCanvasProperty, value); }
        }

        public static readonly DependencyProperty MainCanvasProperty =
           DependencyProperty.Register("MainCanvas", typeof(Canvas), typeof(ReportElement));

        private void button_SaveImage_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ((this.Content as Border).Child as Grid).Children)
            {
                if (item is Image)
                {
                    Image image = item as Image;
                    Tools.ImageHelper.SaveImageToFile((BitmapSource)image.Source);
                }
            }
        }

        private void button_LoadImage_Click(object sender, RoutedEventArgs e)
        {
            foreach (var control in ((this.Content as Border).Child as Grid).Children)
            {
                if (control is Image)
                    (control as Image).Source = Tools.CanvasHelper.SaveCanvasToBitmap(MainCanvas);
            }

            //Tools.CanvasHelper.SaveCanvasToBitmap()
        }

        private void button_DeleteElement(object sender, RoutedEventArgs e)
        {
            var stackPanel = this.Parent as StackPanel;

            var elemIndex = stackPanel.Children.IndexOf(this);

            stackPanel.Children.RemoveAt(elemIndex);

        }
    }
}
