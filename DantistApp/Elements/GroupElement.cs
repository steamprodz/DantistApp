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
    public class GroupElement : Image
    {
        public GroupElement()
        {
            DataContext = this;

            MouseLeftButtonDown += GroupElement_MouseLeftButtonDown;
        }

        public string GroupName
        {
            get { return base.GetValue(GroupNameProperty) as String; }
            set { base.SetValue(GroupNameProperty, value); }
        }

        public static readonly DependencyProperty GroupNameProperty =
           DependencyProperty.Register("GroupName", typeof(String), typeof(GroupElement));


        private void GroupElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
