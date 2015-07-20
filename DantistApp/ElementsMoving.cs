using System;
using System.Collections.Generic;
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
using DantistApp.Elements;

namespace DantistApp
{
    public partial class MainWindow : Window
    {

        //Element _activeElement;
        FrameworkElement _activeElement;

        


        private void MainCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_activeElement != null)
            {
                canvas_main.ReleaseMouseCapture();
                _activeElement = null;
            }
        }


        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_activeElement != null && 
                !(_activeElement as Element).IsFixed)
            {
                bool canMove = true;

                var position = e.GetPosition(canvas_main);
                var offset = position - _mousePosition;
                _mousePosition = position;
                Point destinationPoint = new Point(Canvas.GetLeft(_activeElement) + offset.X,
                                                   Canvas.GetTop(_activeElement) + offset.Y);

                Rect checkRect = new Rect(canvas_main.RenderSize);

                //========* check: canvas contains element *=========
                Rect elementRect = new Rect(destinationPoint.X, destinationPoint.Y,
                                            _activeElement.ActualWidth, _activeElement.ActualHeight);
                checkRect.Intersect(elementRect);
                try
                {
                    if (Enumerable.Range((int)checkRect.Size.Width - 1, 2).Contains((int)elementRect.Size.Width) == false ||
                        Enumerable.Range((int)checkRect.Size.Height - 1, 2).Contains((int)elementRect.Size.Height) == false)
                        canMove = false;
                }
                catch { canMove = false; }
                //===================================================

                checkRect = new Rect(canvas_main.RenderSize);

                //=========* check: canvas contains relative element *==========
                if (_activeElement is CompositeElement)
                {
                    if ((_activeElement as CompositeElement).RelativeElement != null)
                    {
                        CompositeElement relativeElement = (_activeElement as CompositeElement).RelativeElement;
                        Rect relativeElementRect = new Rect(relativeElement.Position.X + offset.X, relativeElement.Position.Y + offset.Y,
                                                            relativeElement.ActualWidth, relativeElement.ActualHeight);
                        //Point relativePos = new Point
                        //{
                        //    X = Canvas.GetLeft(relativeElement),
                        //    Y = Canvas.GetTop(relativeElement)
                        //};
                        relativeElementRect.Location = relativeElement.Position + offset;

                        checkRect.Intersect(relativeElementRect);
                        try
                        {
                            if (Enumerable.Range((int)checkRect.Size.Width - 1, 2).Contains((int)relativeElementRect.Size.Width) == false ||
                                Enumerable.Range((int)checkRect.Size.Height - 1, 2).Contains((int)relativeElementRect.Size.Height) == false)
                                canMove = false;
                        }
                        catch { canMove = false; }
                    }
                }
                //===============================================================

                if (canMove)
                {
                    Point p = new Point(Canvas.GetLeft(_activeElement) + offset.X,
                                        Canvas.GetTop(_activeElement) + offset.Y);
                    (_activeElement as Element).Position = p;
                }
            }
        }


    }
}
