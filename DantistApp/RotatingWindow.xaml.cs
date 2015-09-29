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
                RotateTransform rotateTransform = new RotateTransform(angle, element.ActualWidth / 2, element.ActualHeight / 2);
                element.RenderTransform = rotateTransform;

                CompositeElement compElement = element as CompositeElement;
                if (compElement != null &&
                    compElement.RelativeElement != null &&
                    compElement.IsMerged)
                {
                    CompositeElement relativeElement = compElement.RelativeElement;
                    double height1 = element.ActualHeight - 7;
                    double height2 = 0;
                    if (compElement.CompositeLocation == CompositeLocation.Bot)
                    {
                        height1 = 0;
                        height2 = compElement.RelativeElement.ActualHeight - 7;
                    }
                    RotateTransform rotateTransform1 = new RotateTransform(angle, element.ActualWidth / 2, height1);
                    element.RenderTransform = rotateTransform1;
                    if (compElement.RootSeal != null)
                        compElement.RootSeal.RenderTransform = rotateTransform1;
                    RotateTransform rotateTransform2 = new RotateTransform(angle, relativeElement.ActualWidth / 2, height2);//element.ActualWidth / 2, -(element.ActualHeight + relativeElement.ActualHeight) / 2);
                    compElement.RelativeElement.RenderTransform = rotateTransform2;
                    //RenderTransformOrigin = new Point(element.ActualWidth / 2, (element.ActualHeight + compElement.ActualHeight) / 2);
                }
            }
        }

        private void btn_setDefault_Click(object sender, RoutedEventArgs e)
        {
            Slider_Rotate.Value = 0;
        }


    }
}
