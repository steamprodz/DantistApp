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

        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //var element = e.Source as Element;

            var element = e.Source as FrameworkElement;

            //if (element != null && canvas_main.CaptureMouse() &&
            //    (element is GroupElement || element is UnlimitedElement || element is SingleElement))
            if (element is IManipulatedElement && canvas_main.CaptureMouse())
            {
                _mousePosition = e.GetPosition(canvas_main);
                _activeElement = element;
            }
        }
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
                !(_activeElement as IManipulatedElement).IsFixed)
            {
                var position = e.GetPosition(canvas_main);
                var offset = position - _mousePosition;
                _mousePosition = position;

                Canvas.SetLeft(_activeElement, Canvas.GetLeft(_activeElement) + offset.X);
                Canvas.SetTop(_activeElement, Canvas.GetTop(_activeElement) + offset.Y);
            }
        }


    }
}
