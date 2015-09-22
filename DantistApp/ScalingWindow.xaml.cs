using System;
using System.Collections;
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
using DantistApp.Tools;
using System.Text.RegularExpressions;
using System.Windows.Media.Effects;

namespace DantistApp
{
    /// <summary>
    /// Interaction logic for ScalingWindow.xaml
    /// </summary>
    public partial class ScalingWindow : Window
    {
        public ScalingWindow()
        {
            InitializeComponent();
        }

        private void Slider_Scale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double scale = 0.5 + e.NewValue / 10;

            if (Slider_Scale.Tag is CompositeElement)
            {
                CompositeElement compElement = Slider_Scale.Tag as CompositeElement;
                if (compElement != null)
                {
                    compElement.ChangeSize(scale, true);
                    //if (compElement.RelativeElement != null && 
                    //    compElement.IsMerged)
                    //{
                    //    compElement.RelativeElement.ChangeSize(scale);
                    //}
                }
                else
                {
                    var element = Slider_Scale.Tag as Element;

                    element.Height /= element.Size;
                    element.Width /= element.Size;
                    element.Height *= scale;
                    element.Width *= scale;

                    element.Size = scale;
                }
            }

        }

    }
}
