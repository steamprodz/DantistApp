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

        private void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = e.Source as Element;

            if (element != null && canvas_main.CaptureMouse() &&
                (element is GroupElement || element is UnlimitedElement || element is SingleElement))
            {
                _mousePosition = e.GetPosition(canvas_main);
                _activeElement = element;
            }
        }
        private void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_activeElement != null)
            {
                canvas_main.ReleaseMouseCapture();
                _activeElement = null;
            }
        }
        private void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (_activeElement != null)
            {
                var position = e.GetPosition(canvas_main);
                var offset = position - _mousePosition;
                _mousePosition = position;
                Point p = _activeElement.TranslatePoint(new Point(0, 0), canvas_main);

                //if (_activeElement.Size != null)
                //{
                //    offset /= (double)_activeElement.Size;
                //}
                Canvas.SetLeft(_activeElement, p.X + offset.X);
                Canvas.SetTop(_activeElement, p.Y + offset.Y);
            }
        }


    }
}
