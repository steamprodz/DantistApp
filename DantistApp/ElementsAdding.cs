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

        private void UnlimitedElement_Adding(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                UnlimitedElement element = new UnlimitedElement();
                element.Source = (sender as UnlimitedElement).Source;
                canvas_main.Children.Add(element);
            }
        }

        private void GroupElement_Adding(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                GroupElement basicElement = sender as GroupElement;
                GroupElement sameGroupElement = (from item in canvas_main.Children.OfType<GroupElement>().DefaultIfEmpty()
                                                where item != null && item.GroupName == basicElement.GroupName
                                                select item).FirstOrDefault();

                if (sameGroupElement != null)
                    canvas_main.Children.Remove(sameGroupElement);
                
                GroupElement element = new GroupElement();
                element.Source = basicElement.Source;
                element.GroupName = basicElement.GroupName;
                if (basicElement.StartLocation != null)
                {
                    element.StartLocation = basicElement.StartLocation;
                }
                element.Width = basicElement.Width;
                element.Height = basicElement.Height;
                if (basicElement.Size != 0)
                    element.Size = basicElement.Size;
                canvas_main.Children.Add(element);
            }
        }


    }

}
