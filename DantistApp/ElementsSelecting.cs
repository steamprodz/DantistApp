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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using DantistApp.Elements;
using DantistApp.Tools;

namespace DantistApp
{
    public partial class MainWindow : Window
    {

        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //var element = e.Source as Element;
            
            if (e.Source is Canvas)
            //if (!(e.Source is Element ||
            //        Keyboard.IsKeyDown(Key.LeftShift)))
            {
                ClearSelection();
            }

            var element = e.Source as Element;
            if (element is Element && canvas_main.CaptureMouse())
            {
                _bufferUndoRedo.RecordStateBefore(canvas_main);

                if (Keyboard.IsKeyDown(Key.LeftShift) == false)
                {
                    ClearSelection();
                }

                if (_selectedElements.Contains(element) && Keyboard.IsKeyDown(Key.LeftShift))
                {
                    RemoveFromSelection(element as Element);
                }
                else
                {
                    AddToSelection(element as Element);
                }

                if (element is CompositeElement)
                {
                    CompositeElement relElement = (element as CompositeElement).RelativeElement;
                    if (relElement != null && relElement.IsMerged)
                    {
                        AddToSelection(relElement);
                    }
                }

                _mousePosition = e.GetPosition(canvas_main);
                _activeElement = element;

                _previousActiveElementPos = _mousePosition;
            }
        }


        private void AddToSelection(Element element)
        {
            if (element is CompositeElement)
                label1.Content = (element as CompositeElement).GroupName;
            DropShadowEffect glowEffect = new DropShadowEffect()
            {
                ShadowDepth = 0,
                Color = Colors.GreenYellow,
                Opacity = 1,
                BlurRadius = 20
            };
            element.Effect = glowEffect;
            _selectedElements.Add(element as Element);
        }

        private void RemoveFromSelection(Element element)
        {
            element.Effect = null;
            _selectedElements.Remove(element);
        }

        private void ClearSelection()
        {
            foreach (var item in _selectedElements)
            {
                item.Effect = null;
            }
            _selectedElements.Clear();
        }


    }
}
