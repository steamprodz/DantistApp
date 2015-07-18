using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace DantistApp.Elements
{
    public class Element : Image
    {

        public double? Size
        {
            get { return base.GetValue(SizeProperty) as double?; }
            set 
            {
                if (Size != null && Size != 0)
                {
                    Width /= (double)Size;
                    Height /= (double)Size;
                }
                Width *= (double)value;
                Height *= (double)value;
                base.SetValue(SizeProperty, value); 
            }
        }

        public static readonly DependencyProperty SizeProperty =
           DependencyProperty.Register("Size", typeof(double), typeof(Element));

        public Point StartLocation
        {
            get { return (Point)base.GetValue(StartLocationProperty); }
            set 
            {
                Canvas.SetLeft(this, value.X);
                Canvas.SetTop(this, value.Y);
                base.SetValue(StartLocationProperty, value); 
            }
        }
       

        public static readonly DependencyProperty StartLocationProperty =
           DependencyProperty.Register("StartLocation", typeof(Point), typeof(Element));

    }
}
