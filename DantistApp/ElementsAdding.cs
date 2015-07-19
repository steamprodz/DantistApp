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
using System.Windows.Controls.Primitives;
using DantistApp.Elements;
using DantistApp.Tools;

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
            CompositeElementShell compositeParent = (basicElement.Parent as Grid).Parent as CompositeElementShell;
            if (compositeParent != null)
            {
                #region BRANCH FOR COMPOSITE ELEMENT
                Grid parentCompositeElementGrid = (compositeParent as CompositeElementShell).Content as Grid;
                List<Element> parentElements = new List<Element>();
                foreach (var gridElement in parentCompositeElementGrid.Children)
                {
                    parentElements.Add(gridElement as Element);
                }

                List<CompositeElement> elements = new List<CompositeElement>();

                for (int i = 0; i < parentElements.Count; i++)
                {
                    elements.Add(new CompositeElement());
                    elements[i].Height = parentElements[i].ActualHeight;
                    elements[i].Width = parentElements[i].ActualWidth;
                    elements[i].Source = parentElements[i].Source;
                    elements[i].Size = 1;
                    elements[i].StartLocation = parentElements[i].StartLocation + new Vector(50,50);
                    if (i == 1) elements[i].StartLocation += new Vector(0, compositeParent.ActualHeight - parentElements[1].ActualHeight);
                }
                elements[0].RelativeElement = elements[1];
                elements[1].RelativeElement = elements[0];
                elements[0].IsMerged = true;

                foreach (var item in elements)
                {
                    canvas.Children.Add(item);
                    item.Position = new Point(Canvas.GetLeft(item), Canvas.GetTop(item));
                    AddContextMenu(item, canvas);
                }
                #endregion
            }
            else
            {
                #region BRANCH FOR SINGLE ELEMENT
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
                AddContextMenu(element, canvas);
                #endregion
            }
        }

        /// <summary>
        /// Adds context menu to element according its type
        /// </summary>
        private void AddContextMenu(Element element, Canvas canvas)
        {
            ContextMenu contextMenu = new ContextMenu();

            #region MENU ITEMS DECLARATION
            MenuItem mi_delete = new MenuItem();
            mi_delete.Header = "Удалить элемент";
            mi_delete.Click += //ContextMenu_Delete_Click;
                (object sender, RoutedEventArgs e) =>
                {
                    canvas.Children.Remove(element);
                };

            MenuItem mi_fix = new MenuItem();
            MenuItem mi_unfix = new MenuItem();
            mi_fix.Header = "Зафиксировать элемент";
            mi_unfix.Header = "Отменить фиксацию";
            mi_fix.Click +=
                (object sender, RoutedEventArgs e) =>
                {
                    element.IsFixed = true;
                    element.ContextMenu.Items.Remove(mi_fix);
                    element.ContextMenu.Items.Add(mi_unfix);
                };
            mi_unfix.Click +=
                (object sender, RoutedEventArgs e) =>
                {
                    element.IsFixed = false;
                    element.ContextMenu.Items.Remove(mi_unfix);
                    element.ContextMenu.Items.Add(mi_fix);
                };

            MenuItem mi_merge = new MenuItem();
            MenuItem mi_unmerge = new MenuItem();
            mi_merge.Header = "Объединить элементы";
            mi_unmerge.Header = "Разъединить элементы";
            mi_merge.Click +=
                (object sender, RoutedEventArgs e) =>
                {
                    if (element is CompositeElement)
                    {
                        (element as CompositeElement).IsMerged = true;
                        (element as CompositeElement).ContextMenu.Items.Remove(mi_merge);
                        (element as CompositeElement).ContextMenu.Items.Add(mi_unmerge);
                    }
                };
            mi_unmerge.Click +=
                (object sender, RoutedEventArgs e) =>
                {
                    if (element is CompositeElement)
                    {
                        (element as CompositeElement).IsMerged = false;
                        (element as CompositeElement).ContextMenu.Items.Remove(mi_unmerge);
                        (element as CompositeElement).ContextMenu.Items.Add(mi_merge);
                    }
                };
            #endregion

            if (element is GroupElement)
            {
                contextMenu.Items.Add(mi_delete);
                contextMenu.Items.Add(mi_fix);
            }

            if (element is UnlimitedElement)
            {
                contextMenu.Items.Add(mi_delete);
                contextMenu.Items.Add(mi_fix);
            }

            if (element is CompositeElement)
            {
                contextMenu.Items.Add(mi_delete);
                contextMenu.Items.Add(mi_fix);
                contextMenu.Items.Add(mi_unmerge);
            }

            element.ContextMenu = contextMenu;
        }


        private void ContextMenu_Delete_Click(object sender, RoutedEventArgs e)
        {
            //DependencyObject lol = (((sender as MenuItem).Parent as ContextMenu).Parent as Popup).Parent;
        }
    }

}
