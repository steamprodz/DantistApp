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

        /// <summary>
        /// Double click calls element (of corresponding type) adding
        /// </summary>
        private void Element_AddingOnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Element basicElement = sender as Element;
            bool canAdd = true;

            if (e.ClickCount == 2)
            {

                if (basicElement is GroupElement)
                {
                    GroupElement sameGroupElement = (from item in canvas_main.Children.OfType<GroupElement>().DefaultIfEmpty()
                                                     where item != null && item.GroupName == (basicElement as GroupElement).GroupName
                                                     select item).FirstOrDefault();
                    if (sameGroupElement != null)
                    {
                        if (sameGroupElement.Source == basicElement.Source)
                            canAdd = false;
                        else
                            canvas_main.Children.Remove(sameGroupElement);
                    }
                }

                if (canAdd)
                    AddElement(basicElement, canvas_main);
            }
        }

        /// <summary>
        /// Clones basic element into the new element on canvas
        /// </summary>
        private void AddElement(Element basicElement, Canvas canvas)
        {
            Element element = null;
            if (basicElement is UnlimitedElement)
            {
                element = new UnlimitedElement();
            }
            if (basicElement is GroupElement)
            {
                element = new GroupElement((basicElement as GroupElement).GroupName);
            }
            element.Source = basicElement.Source;
            element.Width = basicElement.Width;
            element.Height = basicElement.Height;
            if (basicElement.Size != 0)
                element.Size = basicElement.Size;
            canvas.Children.Add(element);
            if (basicElement.StartLocation != null)
            {
                element.StartLocation = basicElement.StartLocation;
            }
        }


    }

}
