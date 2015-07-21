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
    // IControlManipulate inherits INotifyPropertyChanged
    public class Element : OpaqueClickableImage, IManipulatedElement
    {
        protected bool _isFixed;
        /// <summary>
        /// Gets or sets a value whether this element is fixed on the canvas.
        /// </summary>
        public bool IsFixed 
        { 
            get { return _isFixed; }
            set { _isFixed = value; } 
        }

        /// <summary>
        /// Event handler for PropertyChange
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged; 

        private double _size;
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

        protected Point _position;
        public virtual Point Position
        {
            get { return _position; }

            set
            {
                _position = value;
                Canvas.SetLeft(this, _position.X);
                Canvas.SetTop(this, _position.Y);
            }
        }

        private Point _startLocation;
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


        public Element CloneIntoCanvas(Canvas canvas, Point point)
        {
            Element element = null;
            if (this is UnlimitedElement)
            {
                element = new UnlimitedElement();
            }
            if (this is GroupElement && !(this is CompositeElement))
            {
                element = new GroupElement((this as GroupElement).GroupName);
            }
            if (this is CompositeElement)
            {
                element = new CompositeElement();

                if (this.Name == "element_bot")
                {
                    var elementShell = (this.Parent as Grid).Parent as CompositeElementShell;
                    point += new Vector(elementShell.HorizontalShift, 0);
                }

            }
            element.IsClone = true;
            element.Source = this.Source;
            element.Width = this.ActualWidth;
            element.Height = this.ActualHeight;
            if (this.Size != 0)
                element.Size = this.Size;
            else
                element.Size = 1;
            
            canvas.Children.Add(element);
            element.Position = point;

            return element;
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
