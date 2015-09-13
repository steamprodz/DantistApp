using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DantistApp.Elements;
using DantistApp.Tools;
using System.Text.RegularExpressions;
using System.Windows.Media.Effects;

namespace DantistApp
{
    /// <summary>
    /// Interaction logic for RotatingWindow.xaml
    /// </summary>
    public partial class RotatingWindow : Window
    {
        public RotatingWindow()
        {
            InitializeComponent();
        }

        private void Slider_Rotate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double angle = e.NewValue;
            Element element = Slider_Rotate.Tag as Element;
            if (element != null)
            {
                RotateTransform rotateTransform = new RotateTransform(angle);
                element.RenderTransform = rotateTransform;

                CompositeElement compElement = element as CompositeElement;
                if (compElement != null &&
                    compElement.RelativeElement != null &&
                    compElement.IsMerged)
                {
                    compElement.RelativeElement.RenderTransform = rotateTransform;
                }
            }
        }

        private void btn_setDefault_Click(object sender, RoutedEventArgs e)
        {
            Slider_Rotate.Value = 0;
        }


    }
}
