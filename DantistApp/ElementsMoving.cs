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
            var image = e.Source as Image;

            if (image != null && canvas_main.CaptureMouse() &&
                (image is GroupElement || image is UnlimitedElement || image is SingleElement))
            {
                _mousePosition = e.GetPosition(canvas_main);
                _activeElement = image;
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
                //Canvas.SetLeft(_activeElement, Canvas.GetLeft(_activeElement) + offset.X);
                //Canvas.SetTop(_activeElement, Canvas.GetTop(_activeElement) + offset.Y);
                Canvas.SetLeft(_activeElement, p.X + offset.X);
                Canvas.SetTop(_activeElement, p.Y + offset.Y);
            }
        }


    }
}
