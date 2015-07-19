using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DantistApp.Elements
{
    interface IManipulatedElement : INotifyPropertyChanged
    {
        double Size { get; set; }
        Point StartLocation { get; set; }
        bool IsFixed { get; set; }
    }
}
