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
                //if (Size != 0)
                //{
                //    Width /= (double)Size;
                //    Height /= (double)Size;
                //}
                //Width *= (double)value;
                //Height *= (double)value;

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


        private int _zIndex;
        public int ZIndex
        {
            get { return _zIndex; }
            set 
            { 
                _zIndex = value; 
                Canvas.SetZIndex(this, _zIndex); 
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

                //if (this.Name == "element_bot")
                //{
                //    var elementShell = (this.Parent as Grid).Parent as CompositeElementShell;
                //    //point += new Vector(elementShell.HorizontalShift, 0);
                //}

            }
            element.IsClone = true;
            element.Source = this.Source;
            element.Width = this.ActualWidth;
            element.Height = this.ActualHeight;
            
            if (this.Size != 0)
            {
                element.Size = this.Size;
                element.Width *= this.Size;
                element.Height *= this.Size;
            }
            else
                element.Size = 1;
            
            canvas.Children.Add(element);

            element.Position = point;
            if (element is CompositeElement && this.Name == "element_bot")
            {
                var elementShell = (this.Parent as Grid).Parent as CompositeElementShell;
                element.Position += new Vector(elementShell.HorizontalShift, 0);
            }

            return element;
        }

        public void ChangeSize(double newSize, bool countingRelative)
        {
            var elementHeightOld = Height;
            var elementWidthOld = Width;

            Width /= Size;
            Height /= Size;
            Width *= newSize;
            Height *= newSize;
            

            if (this is CompositeElement && (this as CompositeElement).RootSeal != null)
            {
                var composite = this as CompositeElement;

                composite.RootSeal.Width /= Size;
                composite.RootSeal.Height /= Size;
                composite.RootSeal.Width *= newSize;
                composite.RootSeal.Height *= newSize;
            }

            Size = newSize;

            if (countingRelative && this is CompositeElement)
            {
                CompositeElement compElement = this as CompositeElement;
                CompositeElement relElement = compElement.RelativeElement;
                
                if (relElement != null && compElement.IsMerged)
                {
                    var relElementHeightOld = relElement.Height;
                    var relElementWidthOld = relElement.Width;

                    relElement.Width /= relElement.Size;
                    relElement.Height /= relElement.Size;
                    relElement.Width *= newSize;
                    relElement.Height *= newSize;
                    relElement.Size = newSize;



                    //была писечка до фикса, но дьяк забил
                    //Vector compElementPos = new Vector(compElement.Position.X, compElement.Position.Y);
                    //Vector relElementPos = new Vector(relElement.Position.X, relElement.Position.Y);

                    //Vector offset = relElementPos - compElementPos;
                    //relElementPos -= offset;
                    //relElementPos += offset * newSize;

                    //relElement.SetPositionDirectly(new Point(relElementPos.X, relElementPos.Y));
                    
                    
                    var dyRelative = relElement.Height - relElementHeightOld;
                    var dxRelative = relElement.Width - relElementWidthOld;

                    var dy = Height - elementHeightOld;
                    var dx = Width - elementWidthOld;

                    if (compElement.CompositeLocation == CompositeLocation.Top)
                    {
                        Canvas.SetTop(relElement, Canvas.GetTop(relElement) + dy);

                        //if (compElement.GroupName.ToString().Substring(5, 1) == "1" || compElement.GroupName.ToString().Substring(5, 1) == "2")
                        //{
                            if (compElement.RootSeal != null)
                            {
                                Canvas.SetTop(compElement.RootSeal, Canvas.GetTop(compElement.RootSeal) + dyRelative);
                                Canvas.SetLeft(compElement.RootSeal, Canvas.GetLeft(compElement.RootSeal) + dxRelative / 8);
                            }
                        //}
                    }
                    else
                    {
                        Canvas.SetTop(compElement, Canvas.GetTop(compElement) + dyRelative);

                        //if (compElement.GroupName.ToString().Substring(5, 1) == "3" || compElement.GroupName.ToString().Substring(5, 1) == "4")
                        //{
                            if (compElement.RootSeal != null)
                            {
                                Canvas.SetTop(compElement.RootSeal, Canvas.GetTop(compElement.RootSeal) + dy);
                                Canvas.SetLeft(compElement.RootSeal, Canvas.GetLeft(compElement.RootSeal) + dx / 8);
                            }
                        //}
                    }
                    //Canvas.SetLeft(relElement, Canvas.GetLeft(relElement) + dxRelative);
                    
                    //Canvas.SetTop(compElement, Canvas.GetTop(compElement) - (dy + dyRelative) / 2);
                    //Canvas.SetLeft(compElement, Canvas.GetLeft(compElement) + dx);

                    
                }

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
