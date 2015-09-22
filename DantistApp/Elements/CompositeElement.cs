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
    public enum CompositeLocation
    {
        Top,
        Bot
    }
    
    public class CompositeElement : GroupElement
    {

        //public EventHandler CompositeRemoved;

        //private void HandleCompositeRemoved(object sender, EventArgs e)
        //{
        //    var composite = sender as CompositeElement;

        //    if (composite.RootSeal != null)
        //        Canvas.
        //}

        public CompositeElement RelativeElement
        {
            get;
            set;
        }

        // Для корня - пломбирование
        public Element RootSeal { get; set; }

        public double HorizontalShift { get; set; }

        public Label RelativeToothNumber { get; set; }

        private bool _isMerged;
        public bool IsMerged
        {
            get { return _isMerged; }
            set
            {
                _isMerged = value;
                if (RelativeElement != null)
                    RelativeElement._isMerged = value;

                if (RelativeToothNumber != null)
                {
                    if (IsMerged)
                    {
                        if (CompositeLocation == Elements.CompositeLocation.Bot)
                            RelativeToothNumber.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else
                    {
                        RelativeToothNumber.Visibility = System.Windows.Visibility.Visible;
                        Canvas.SetLeft(RelativeToothNumber, Position.X);
                        Canvas.SetTop(RelativeToothNumber, Position.Y);
                    }
                }
                
            }
        }


        public override Point Position
        {
            get { return _position; }
            set
            {
                if (IsMerged && _position != new Point(0,0))
                {
                    Canvas.SetLeft(RelativeElement, RelativeElement.Position.X + value.X - _position.X);
                    Canvas.SetTop(RelativeElement, RelativeElement.Position.Y + value.Y - _position.Y);
                    RelativeElement._position = new Point(RelativeElement.Position.X + value.X - _position.X,
                                                            RelativeElement.Position.Y + value.Y - _position.Y);
                   
                }
                Canvas.SetLeft(this, value.X);
                Canvas.SetTop(this, value.Y);

                if (RootSeal != null)
                {
                    Canvas.SetLeft(RootSeal, Canvas.GetLeft(RootSeal) + value.X - _position.X);
                    Canvas.SetTop(RootSeal, Canvas.GetTop(RootSeal) + value.Y - _position.Y);
                }

                if (RelativeToothNumber != null)
                {
                    Canvas.SetLeft(RelativeToothNumber, value.X);
                    Canvas.SetTop(RelativeToothNumber, value.Y);
                    if (CompositeLocation == Elements.CompositeLocation.Bot && RelativeElement != null)
                    {
                        Canvas.SetLeft(RelativeElement.RelativeToothNumber, RelativeElement.Position.X);
                        Canvas.SetTop(RelativeElement.RelativeToothNumber, RelativeElement.Position.Y);
                    }
                }

                //if (RootSeal != null)
                //{
                //    Canvas.SetLeft(RootSeal, RootSeal.Position.X + value.X - _position.X);
                //    Canvas.SetTop(RootSeal, RootSeal.Position.Y + value.Y - _position.Y);
                //}

                _position = value;
            }
        }

        /// <summary>
        /// Sets position without changing relative element position
        /// </summary>
        public void SetPositionDirectly(Point pos)
        {
            _position = pos;
            Canvas.SetLeft(this, pos.X);
            Canvas.SetTop(this, pos.Y);
        }


        public void Replace(CompositeElement newElement, Vector oldShift)
        {
            Canvas canvas = Parent as Canvas;
            if (canvas == null)
                return;
            CompositeElement oldElement = this;
            CompositeElement oldRelElement = oldElement.RelativeElement;
            CompositeElement newRelElement = newElement.RelativeElement;

            newElement.Position = oldElement.Position;
            var dx = newElement.Width - oldElement.Width;
            var dy = newElement.Height - oldElement.Height;

            if (oldElement.CompositeLocation == Elements.CompositeLocation.Bot &&
                newElement.Source != oldElement.Source)
            {
                newElement.Position -= new Vector(oldElement.HorizontalShift, 0);
                newElement.Position += oldShift;
            }

            if (oldElement.CompositeLocation == Elements.CompositeLocation.Bot)
            {
                newElement.Position -= new Vector(dx, 0);
            }

            if (oldElement.RootSeal != null)
            {
                canvas.Children.Remove(oldElement.RootSeal);
                oldElement.RootSeal = null;
            }

            //Image/lunky_plastyka/48.png 21 14
            //if (newElement.Source.ToString().Substring(newElement.Source.ToString().Length - 21, 14) == "lunky_plastyka"
            //    || newElement.Source.ToString().Substring(newElement.Source.ToString().Length - 20, 14) == "lunky_plastyka")
            if (newElement.Source.ToString().Contains("lunky_plastyka"))
            {
                if (newElement is CompositeElement)
                {
                    if (oldElement.CompositeLocation == CompositeLocation.Top 
                        && (oldElement.GroupName.Substring(5, 1) == "1" || oldElement.GroupName.Substring(5, 1) == "2"))
                    {
                        if (oldElement.RelativeElement != null)
                            canvas.Children.Remove(oldElement.RelativeElement);
                    }
                    else if (oldElement.CompositeLocation == CompositeLocation.Bot
                        && (oldElement.GroupName.Substring(5, 1) == "3" || oldElement.GroupName.Substring(5, 1) == "4"))
                    {
                        if (oldElement.RelativeElement != null)
                            canvas.Children.Remove(oldElement.RelativeElement);
                    }
                }
            }
                //newElement.Position -= new Vector(dx / 2, dy);
            
            //if (newRelElement == null && newElement.Source != oldElement.Source)
            //    //newElement.Position = new Point(100, 100);
            //    newElement.Position = oldElement.Position - oldShift;
            if (oldRelElement != null)
            {
                newElement.RelativeElement = oldRelElement;
                oldRelElement.RelativeElement = newElement;

                newElement.IsMerged = oldRelElement.IsMerged;
            }
            canvas.Children.Remove(oldElement);
        }

        public CompositeLocation CompositeLocation
        {
            get { return (CompositeLocation)base.GetValue(CompositeLocationProperty); }
            set { base.SetValue(CompositeLocationProperty, value); }
        }

        public static readonly DependencyProperty CompositeLocationProperty =
           DependencyProperty.Register("CompositeLocation", typeof(CompositeLocation), typeof(CompositeElement));

    }

}
