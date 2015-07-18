﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DantistApp.Elements
{
    interface IControlManipulate : INotifyPropertyChanged
    {
        double Size { get; set; }
        Point StartLocation { get; set; }
    }
}
