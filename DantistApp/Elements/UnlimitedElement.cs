﻿using System;
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
    public class UnlimitedElement : Image
    {
        public UnlimitedElement()
        {
            DataContext = this;

            MouseLeftButtonDown += UnlimitedElement_MouseLeftButtonDown;
        }


        private void UnlimitedElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
