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
using System.Text.RegularExpressions;
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

            if (e.ClickCount == 2)
            {
                AddElementToCanvas(basicElement, true);
            }
        }

        public void AddElementToCanvas(Element basicElement, bool recordInBuffer)
        {
            bool canAdd = true;

            if (basicElement is GroupElement && !(basicElement is CompositeElement))
            {
                GroupElement sameGroupElement = (from item in CanvasMain.Children.OfType<GroupElement>().DefaultIfEmpty()
                                                 where item != null && !(item is CompositeElement) &&
                                                 item.GroupName == (basicElement as GroupElement).GroupName
                                                 select item).FirstOrDefault();
                if (sameGroupElement != null)
                {
                    if (sameGroupElement.Source == basicElement.Source)
                        canAdd = false;
                    else
                        CanvasMain.Children.Remove(sameGroupElement);
                }
            }

            if (canAdd)
            {
                if (recordInBuffer)
                    _bufferUndoRedo.RecordStateBefore(CanvasMain);

                AddToCanvas(basicElement, CanvasMain);

                if (recordInBuffer)
                    _bufferUndoRedo.RecordStateAfter(CanvasMain);
            }
        }

        /// <summary>
        /// Clones basic element into the canvas
        /// </summary>
        private void AddToCanvas(Element basicElement, Canvas canvas)
        {
            CompositeElementShell compositeShell = (basicElement.Parent as Grid).Parent as CompositeElementShell;
            
            if (compositeShell != null)
            {
                List<CompositeElement> elements = null;
                bool addBothParts = (compositeShell.element_bot.Source != null &&
                                     compositeShell.element_top.Source != null);
                elements = AddCompositeElement(basicElement, canvas);
            }
            else
            {
                Element element = AddSingleElement(basicElement, canvas);
            }
        }


        private Element AddSingleElement(Element basicElement, Canvas canvas)
        {
            Element element = basicElement.CloneIntoCanvas(canvas, basicElement.StartLocation);
            AddContextMenu(element, canvas);

            return element;
        }

        private List<CompositeElement> AddCompositeElement(Element basicElement, Canvas canvas)
        {
            CompositeElementShell compositeShell = (basicElement.Parent as Grid).Parent as CompositeElementShell;
            Grid parentCompositeElementGrid = (compositeShell as CompositeElementShell).Content as Grid;
            List<Element> parentElements = new List<Element>();
            foreach (var gridElement in parentCompositeElementGrid.Children)
            {
                parentElements.Add(gridElement as Element);
            }

            List<CompositeElement> elements = new List<CompositeElement>();

            for (int i = 0; i < parentElements.Count; i++)
            {
                Point startPos = compositeShell.StartLocation;
                if (startPos.X == 0 &&
                    startPos.Y == 0)
                    startPos = new Point(50, 50);

                if (parentElements[i].Source != null)
                {
                    CompositeElement element = new CompositeElement();

                    // ща как накостыляю
                    // впиливаем в статику нужный нам номер зуба
                    ImageSource source;
                    string groupName;

                    if (compositeShell.SourceBot != null)
                    {
                        source = compositeShell.SourceBot;
                        groupName = "ToothBot";
                    }
                    else
                    {
                        source = compositeShell.SourceTop;
                        groupName = "ToothTop";
                    }

                    (parentElements[i] as CompositeElement).GroupName = "tooth" + Convert.ToInt32(Regex.Match(source.ToString(), @"\d+").Value);

                    element = parentElements[i].CloneIntoCanvas(canvas, startPos) as CompositeElement;
                  
                    if (parentElements[i] == compositeShell.element_bot)
                    {
                        element.Position += new Vector(compositeShell.ActualWidth - parentElements[1].ActualWidth,
                                                           compositeShell.ActualHeight - parentElements[1].ActualHeight);
                        element.CompositeLocation = CompositeLocation.Bot;
                        //element.GroupName = "tooth" + Convert.ToInt32(Regex.Match(compositeShell.SourceBot.ToString(), @"\d+").Value);
                        element.GroupName = (parentElements[i] as CompositeElement).GroupName;
                        element.HorizontalShift = compositeShell.HorizontalShift;
                    }
                    if (parentElements[i] == compositeShell.element_top)
                    {
                        element.CompositeLocation = CompositeLocation.Top;
                        element.GroupName = "tooth" + Convert.ToInt32(Regex.Match(compositeShell.SourceTop.ToString(), @"\d+").Value);
                    }
                    elements.Add(element);

                }
            }
            if (elements.Count > 1)
            {
                elements[0].RelativeElement = elements[1];
                elements[1].RelativeElement = elements[0];
                elements[0].IsMerged = true;
            }

            foreach (CompositeElement element in elements)
            {
                if (element.CompositeLocation == CompositeLocation.Bot)
                {
                    CompositeElement sameCompositeBot = SameCompositeBot(element, canvas);
                    if (sameCompositeBot != null)
                    {
                        if (elements.Count > 1)
                            canvas.Children.Remove(sameCompositeBot);
                            //sameCompositeBot.Position = element.Position;
                        sameCompositeBot.Replace(element, new Vector(compositeShell.HorizontalShift, 0));
                        //ReplaceCompositeElement(canvas, sameCompositeBot, element);
                    }
                }
                if (element.CompositeLocation == CompositeLocation.Top)
                {
                    CompositeElement sameCompositeTop = SameCompositeTop(element, canvas);
                    if (sameCompositeTop != null)
                    {
                        if (elements.Count > 1)
                            canvas.Children.Remove(sameCompositeTop);
                            //sameCompositeTop.Position = element.Position;
                        sameCompositeTop.Replace(element, new Vector(0, 0));
                        //ReplaceCompositeElement(canvas, sameCompositeTop, element);
                    }
                }
                AddContextMenu(element, canvas);
            }
            return elements;
        }

        private CompositeElement SameCompositeBot(CompositeElement element, Canvas canvas)
        {
            List<CompositeElement> sameCompositeElement = (from item in canvas.Children.OfType<CompositeElement>().DefaultIfEmpty()
                                                           where item != null && item != element &&
                                                           item.GroupName != String.Empty &&
                                                           item.GroupName == (element as CompositeElement).GroupName
                                                           select item).ToList();
            CompositeElement sameCompositeBot = (from item in sameCompositeElement
                                                 where item.CompositeLocation == CompositeLocation.Bot
                                                 select item).FirstOrDefault();
            return sameCompositeBot;
        }
        private CompositeElement SameCompositeTop(CompositeElement element, Canvas canvas)
        {
            List<CompositeElement> sameCompositeElement = (from item in canvas.Children.OfType<CompositeElement>().DefaultIfEmpty()
                                                           where item != null && item != element &&
                                                           item.GroupName != String.Empty &&
                                                           item.GroupName == (element as CompositeElement).GroupName
                                                           select item).ToList();
            CompositeElement sameCompositeTop = (from item in sameCompositeElement
                                                 where item.CompositeLocation == CompositeLocation.Top
                                                 select item).FirstOrDefault();
            return sameCompositeTop;
        }

        //private void ReplaceCompositeElement(Canvas canvas, CompositeElement oldElement, CompositeElement newElement)
        //{
        //    CompositeElement oldRelElement = oldElement.RelativeElement;
        //    CompositeElement newRelElement = newElement.RelativeElement;
        //    if (newRelElement == null)
        //        newElement.Position = oldElement.Position;
        //    if (oldRelElement != null)
        //    {
        //        newElement.RelativeElement = oldRelElement;
        //        oldRelElement.RelativeElement = newElement;

        //        newElement.IsMerged = oldRelElement.IsMerged;
        //    }
        //    canvas.Children.Remove(oldElement);
        //}


        /// <summary>
        /// Adds context menu to element according its type
        /// </summary>
        private void AddContextMenu(Element element, Canvas canvas)
        {
            if (element is GroupElement && !(element is CompositeElement))
            {
                AddMenuItem(element, MenuItemType.Remove);
                AddMenuItem(element, MenuItemType.Fix);
            }

            if (element is UnlimitedElement)
            {
                AddMenuItem(element, MenuItemType.Remove);
                AddMenuItem(element, MenuItemType.Fix);
            }

            if (element is CompositeElement)
            {
                AddMenuItem(element, MenuItemType.Remove);
                AddMenuItem(element, MenuItemType.Fix);
                AddMenuItem(element, MenuItemType.Merge);
                AddMenuItem(element, MenuItemType.Scaling);
               // AddMenuItem(element, MenuItemType.Replace);
            }
        }


        /// <summary>
        /// Removes element from canvas
        /// </summary>
        private void RemoveElement(Element element, Canvas canvas)
        {
            //BufferAction bufAct = new BufferAction();
            //bufAct.Do += () =>
            //{
            //    canvas.Children.Remove(element);
            //};
            //bufAct.Undo += () =>
            //{
            //    canvas.Children.Add(element);
            //};
            //_bufferUndoRedo_Old.StartAction(bufAct);
            _bufferUndoRedo.RecordStateBefore(canvas);

            if (element is CompositeElement)
            {
                CompositeElement compElement = element as CompositeElement;
                if (compElement.RelativeToothNumber != null)
                {
                    canvas.Children.Remove(compElement.RelativeToothNumber);
                }
                // костылэйшн: удаление всего композита вместо частей
                if (compElement.IsMerged == true)
                {
                    canvas.Children.Remove(compElement);
                    canvas.Children.Remove(compElement.RelativeElement);
                    if (compElement.RelativeElement.RelativeToothNumber != null)
                        canvas.Children.Remove(compElement.RelativeElement.RelativeToothNumber);
                }
                else
                {
                    canvas.Children.Remove(compElement);
                }
            }
            _bufferUndoRedo.RecordStateAfter(canvas);
        }

    }

}
