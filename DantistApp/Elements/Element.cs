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

namespace DantistApp.Elements
{
    // IControlManipulate inherits INotifyPropertyChanged
    public class Element : Image, IControlManipulate
    {
        /// <summary>
        /// Gets or sets a value whether this element is fixed on the canvas.
        /// </summary>
        public bool IsFixed { get; set; }

        /// <summary>
        /// Event handler for PropertyChange
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged; 

        private double _size;
        private Point _startLocation;

        public double Size
        {
            get { return _size; }

            set
            {
                if (Size != 0)
                {
                    Width /= (double)Size;
                    Height /= (double)Size;
                }
                Width *= (double)value;
                Height *= (double)value;

                _size = value;

                // TODO: Raise the NotifyPropertyChanged event here
                OnPropertyChanged("Size");
            }
        }

        public Point StartLocation
        {
            get { return _startLocation; }
            
            set
            {
                Canvas.SetLeft(this, value.X);
                Canvas.SetTop(this, value.Y);
                
                _startLocation = value;

                // TODO: Raise the NotifyPropertyChanged event here
                OnPropertyChanged("StartLocation");
            }
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
