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

namespace DantistApp
{
    public enum ReplaceLocation
    {
        Top,
        Down
    }

    public class SingleElement : Image
    {

        public SingleElement()
        {
            DataContext = this;

            MouseLeftButtonDown += ReplacingElement_MouseLeftButtonDown;
        }

        public ReplaceLocation? ReplaceLocation
        {
            get { return base.GetValue(ReplaceLocationProperty) as ReplaceLocation?; }
            set { base.SetValue(ReplaceLocationProperty, value); }
        }

        public static readonly DependencyProperty ReplaceLocationProperty =
           DependencyProperty.Register("ReplaceLocation", typeof(ReplaceLocation?), typeof(GroupElement));


        private void ReplacingElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                //...
                //add element
                //...
            }
        }


    }

}
