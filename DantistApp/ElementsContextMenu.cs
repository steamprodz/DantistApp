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
using System.Text.RegularExpressions;
using DantistApp.Elements;
using DantistApp.Tools;

namespace DantistApp
{
    public partial class MainWindow : Window
    {

        private enum MenuItemType
        {
            Remove,
            Fix,
            Merge,
            Replace,
            Scaling,
            MakeBg,
            MakeFg,
            LayerDown,
            LayerUp,
            Rotate,
            InsufficientlySealed,
            Sealed
        }

        const string MENU_ITEM_NAME_REMOVE = "Удалить элемент";
        const string MENU_ITEM_NAME_FIX = "Зафиксировать элемент";
        const string MENU_ITEM_NAME_UNFIX = "Отменить фиксацию";
        const string MENU_ITEM_NAME_MERGE = "Объединить элементы";
        const string MENU_ITEM_NAME_UNMERGE = "Разъединить элементы";
        const string MENU_ITEM_NAME_REPLACE = "Замена";
        const string MENU_ITEM_NAME_SCALING = "Масштабирование";
        const string MENU_ITEM_NAME_MAKE_BACKGROUND = "Вынести на задний план";
        const string MENU_ITEM_NAME_MAKE_FOREGROUND = "Вынести на передний план";
        const string MENU_ITEM_NAME_LAYER_DOWN = "На слой ниже";
        const string MENU_ITEM_NAME_LAYER_UP = "На слой выше";
        const string MENU_ITEM_NAME_ROTATE = "Повернуть";
        const string MENU_ITEM_NAME_INSUF_SEALED = "Канал недостаточно запломбирован";
        const string MENU_ITEM_NAME_SEALED = "Канал запломбирован";

        private void AddMenuItem(Element element, MenuItemType menuItemType)
        {
            ContextMenu contextMenu = element.ContextMenu;
            Canvas canvas = element.Parent as Canvas;
            if (canvas == null)
                return;
            if (contextMenu == null)
                contextMenu = new ContextMenu();
            switch (menuItemType)
            {
                case MenuItemType.Remove:
                    MenuItem mi_remove = new MenuItem();
                    InitMenuItem_Remove(mi_remove, element, canvas);
                    contextMenu.Items.Add(mi_remove);
                    break;
                case MenuItemType.Fix:
                    MenuItem mi_fix = new MenuItem();
                    MenuItem mi_unfix = new MenuItem();
                    InitMenuItem_Fix(mi_fix, element, canvas, mi_unfix);
                    InitMenuItem_Unfix(mi_unfix, element, canvas, mi_fix);

                    if (element is CompositeElement)
                        contextMenu.Items.Add(mi_fix);
                    else
                        contextMenu.Items.Add(mi_unfix);

                    break;
                case MenuItemType.Merge:
                    MenuItem mi_merge = new MenuItem();
                    MenuItem mi_unmerge = new MenuItem();
                    InitMenuItem_Merge(mi_merge, element, canvas, mi_unmerge);
                    InitMenuItem_Unmerge(mi_unmerge, element, canvas, mi_merge);
                    contextMenu.Items.Add(mi_unmerge);
                    break;
                case MenuItemType.Replace:
                    MenuItem mi_replace = new MenuItem();
                    InitMenuItem_Replace(mi_replace, element, canvas);
                    contextMenu.Items.Add(mi_replace);
                    break;
                case MenuItemType.Scaling:
                    MenuItem mi_scaling = new MenuItem();
                    InitMenuItem_Scaling(mi_scaling, element, canvas);
                    contextMenu.Items.Add(mi_scaling);
                    break;
                case MenuItemType.MakeBg:
                    MenuItem mi_makeBg = new MenuItem();
                    InitMenuItem_MakeBackground(mi_makeBg, element, canvas);
                    contextMenu.Items.Add(mi_makeBg);
                    break;
                case MenuItemType.MakeFg:
                    MenuItem mi_makeFg = new MenuItem();
                    InitMenuItem_MakeForeground(mi_makeFg, element, canvas);
                    contextMenu.Items.Add(mi_makeFg);
                    break;
                case MenuItemType.LayerDown:
                    MenuItem mi_layerDown = new MenuItem();
                    InitMenuItem_LayerDown(mi_layerDown, element, canvas);
                    contextMenu.Items.Add(mi_layerDown);
                    break;
                case MenuItemType.LayerUp:
                    MenuItem mi_layerUp = new MenuItem();
                    InitMenuItem_LayerUp(mi_layerUp, element, canvas);
                    contextMenu.Items.Add(mi_layerUp);
                    break;
                case MenuItemType.Rotate:
                    MenuItem mi_rotate = new MenuItem();
                    InitMenuItem_Rotate(mi_rotate, element, canvas);
                    contextMenu.Items.Add(mi_rotate);
                    break;
                case MenuItemType.InsufficientlySealed:
                    MenuItem mi_insufficientlySealed = new MenuItem();
                    InitMenuItem_InsufficientlySealed(mi_insufficientlySealed, element, canvas);
                    contextMenu.Items.Add(mi_insufficientlySealed);
                    break;
                case MenuItemType.Sealed:
                    MenuItem mi_sealed = new MenuItem();
                    InitMenuItem_Sealed(mi_sealed, element, canvas);
                    contextMenu.Items.Add(mi_sealed);
                    break;

                default: break;
            }
            if (element.ContextMenu == null)
                element.ContextMenu = contextMenu;
        }




        //==============================================================================================
        #region MENU_ITEMS
        private void InitMenuItem_Remove(MenuItem mi_remove, Element element, Canvas canvas)
        {
            mi_remove.Header = MENU_ITEM_NAME_REMOVE;
            mi_remove.Click +=
                (object sender, RoutedEventArgs e) =>
                {
                    RemoveElement(element, canvas);
                };
        }


        private void InitMenuItem_Fix(MenuItem mi_fix, Element element, Canvas canvas, MenuItem mi_unfix)
        {
            mi_fix.Header = MENU_ITEM_NAME_FIX;
            mi_fix.Click +=
                (object sender, RoutedEventArgs e) =>
                {

                    _bufferUndoRedo.RecordStateBefore(CanvasMain);

                    element.IsFixed = true;
                    Helpers.ReplaceMenuItem(mi_fix, mi_unfix);
                    if (element is CompositeElement)
                    {
                        CompositeElement compElement = element as CompositeElement;
                        if (compElement.RelativeElement != null)
                        {
                            MenuItem relative_mi_fix = FindMenuItem(compElement.RelativeElement.ContextMenu, MENU_ITEM_NAME_FIX);
                            if (compElement.IsMerged && relative_mi_fix != null)
                            {
                                relative_mi_fix.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                                _bufferUndoRedo.UndoStack.Pop();
                            }
                        }
                    }

                    _bufferUndoRedo.RecordStateAfter(CanvasMain);

                };
        }

        private void InitMenuItem_Unfix(MenuItem mi_unfix, Element element, Canvas canvas, MenuItem mi_fix)
        {
            mi_unfix.Header = MENU_ITEM_NAME_UNFIX;
            mi_unfix.Click +=
                (object sender, RoutedEventArgs e) =>
                {
                    _bufferUndoRedo.RecordStateBefore(CanvasMain);

                    element.IsFixed = false;
                    Helpers.ReplaceMenuItem(mi_unfix, mi_fix);
                    if (element is CompositeElement)
                    {
                        CompositeElement compElement = element as CompositeElement;
                        if (compElement.RelativeElement != null)
                        {
                            MenuItem relative_mi_unfix = FindMenuItem(compElement.RelativeElement.ContextMenu, MENU_ITEM_NAME_UNFIX);
                            if (compElement.IsMerged && relative_mi_unfix != null)
                            {
                                relative_mi_unfix.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                                _bufferUndoRedo.UndoStack.Pop();
                            }
                        }
                    }

                    _bufferUndoRedo.RecordStateAfter(CanvasMain);

                };
        }



        private void InitMenuItem_Merge(MenuItem mi_merge, Element element, Canvas canvas, MenuItem mi_unmerge)
        {
            mi_merge.Header = MENU_ITEM_NAME_MERGE;
            mi_merge.Click +=
                (object sender, RoutedEventArgs e) =>
                {
                    if (element is CompositeElement == false)
                        return;

                    _bufferUndoRedo.RecordStateBefore(CanvasMain);

                    CompositeElement compElement = element as CompositeElement;
                    compElement.IsMerged = true;
                    Helpers.ReplaceMenuItem(mi_merge, mi_unmerge);
                    if (compElement.RelativeElement != null)
                    {
                        MenuItem relative_mi_merge = FindMenuItem(compElement.RelativeElement.ContextMenu, MENU_ITEM_NAME_MERGE);
                        if (relative_mi_merge != null)
                        {
                            relative_mi_merge.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                            _bufferUndoRedo.UndoStack.Pop();
                        }
                    }

                    _bufferUndoRedo.RecordStateAfter(CanvasMain);
                };
        }

        private void InitMenuItem_Unmerge(MenuItem mi_unmerge, Element element, Canvas canvas, MenuItem mi_merge)
        {
            mi_unmerge.Header = MENU_ITEM_NAME_UNMERGE;
            mi_unmerge.Click +=
                (object sender, RoutedEventArgs e) =>
                {
                    if (element is CompositeElement == false)
                        return;

                    _bufferUndoRedo.RecordStateBefore(CanvasMain);

                    CompositeElement compElement = element as CompositeElement;
                    compElement.IsMerged = false;
                    Helpers.ReplaceMenuItem(mi_unmerge, mi_merge);
                    if (compElement.RelativeElement != null)
                    {
                        MenuItem relative_mi_unmerge = FindMenuItem(compElement.RelativeElement.ContextMenu, MENU_ITEM_NAME_UNMERGE);
                        if (relative_mi_unmerge != null)
                        {
                            relative_mi_unmerge.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                            _bufferUndoRedo.UndoStack.Pop();
                        }
                    }

                    _bufferUndoRedo.RecordStateAfter(CanvasMain);
                };
        }


        private void InitMenuItem_Replace(MenuItem mi_replace, Element element, Canvas canvas)
        {
            //MenuItem mi_botPart = new MenuItem();
            //MenuItem mi_topPart = new MenuItem();

            List<Grid> grids = new List<Grid>();
            List<Element> elements = Helpers.GetLogicalChildCollection<Element>(tabControl_elements);
            foreach (var item in elements)
            {
                if (item is CompositeElement)
                {
                    CompositeElement currentElement = element as CompositeElement;
                    CompositeElement foundBasicElement = item as CompositeElement;
                    CompositeElementShell shell = (foundBasicElement.Parent as Grid).Parent as CompositeElementShell;
                    int numFound = 0;
                    if (shell.SourceBot != null)
                        numFound = Convert.ToInt32(Regex.Match(shell.SourceBot.ToString(), @"\d+").Value);
                    else
                        numFound = Convert.ToInt32(Regex.Match(shell.SourceTop.ToString(), @"\d+").Value);
                    int numCurrent = Convert.ToInt32(Regex.Match(currentElement.GroupName.ToString(), @"\d+").Value);

                    if (numFound == numCurrent &&
                        currentElement.CompositeLocation == foundBasicElement.CompositeLocation)
                    {
                        string extraTabName = String.Empty;
                        int num_tabItem = 0;
                        // int num_extraTabItem = 0;
                        TabItem tabItemParent = FindParent_TabItem(foundBasicElement);
                        if (FindParent_TabItem(tabItemParent) == null)
                        {
                            num_tabItem = Convert.ToInt32(tabItemParent.Header) - 1;
                        }
                        else
                        {
                            num_tabItem = Convert.ToInt32(FindParent_TabItem(tabItemParent).Header) - 1;
                            extraTabName = ": " + tabItemParent.Header.ToString();
                        }
                        //try
                        //{
                        //    //num_tabItem = Convert.ToInt32(tabItemParent.Header) - 1;
                        //}
                        //catch
                        //{
                        //    num_tabItem = Convert.ToInt32(FindParent_TabItem(tabItemParent).Header) - 1;
                        //    extraTabName = ": " + tabItemParent.Header.ToString();
                        //}
                        MenuItem mi = new MenuItem();
                        mi_replace.Items.Add(mi);
                        Button foundBtn = panel_btns.Children[num_tabItem] as Button;
                        mi.Header = foundBtn.Content + extraTabName;
                        mi.Tag = num_tabItem;

                        mi.Click +=
                            (object sender, RoutedEventArgs e) =>
                            {
                                //Element current = currentElement;
                                _bufferUndoRedo.RecordStateBefore(canvas);
                                List<CompositeElement> el = AddCompositeElement(foundBasicElement, canvas);
                                // AddSingleElement(foundBasicElement, canvas);
                                _bufferUndoRedo.RecordStateAfter(canvas);
                            };
                        //if (currentElement.CompositeLocation == CompositeLocation.Bot)
                        //    mi_botPart.Items.Add(mi);
                        //if (currentElement.CompositeLocation == CompositeLocation.Top)
                        //    mi_topPart.Items.Add(mi);
                    }
                }
            }
            mi_replace.Header = MENU_ITEM_NAME_REPLACE;
            mi_replace.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Tag",
                    System.ComponentModel.ListSortDirection.Ascending));
        }


        private void InitMenuItem_Scaling(MenuItem mi_scaling, Element element, Canvas canvas)
        {
            mi_scaling.Header = MENU_ITEM_NAME_SCALING;
            mi_scaling.Click +=
                (object sender, RoutedEventArgs e) =>
                {
                    //if (element is CompositeElement == false)
                    //    return;

                    CompositeElement compElement = element as CompositeElement;

                    if (ScalingWindow != null)
                        ScalingWindow.Close();
                    ScalingWindow = new ScalingWindow();
                    //scalingWnd.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
                    ScalingWindow.Left = this.Left + element.Position.X - ScalingWindow.Width / 2;
                    ScalingWindow.Top = this.Top + element.Position.Y - 50;
                    string title = "Масштабирование";
                    if (compElement != null)
                        title = "Масштабирование (зуб №" + Convert.ToInt32(Regex.Match(compElement.Source.ToString(), @"\d+").Value) + ")";
                    ScalingWindow.Title = title;
                    ScalingWindow.Show();
                    RefreshScalingLine(ScalingWindow, element, canvas);

                    ScalingWindow.Slider_Scale.ValueChanged += (object sender_slider, RoutedPropertyChangedEventArgs<double> e_slider) =>
                    {
                        RefreshScalingLine(ScalingWindow, ScalingWindow.Slider_Scale.Tag as Element, canvas);
                    };
                    ScalingWindow.LocationChanged += (object sender_scalingWnd, EventArgs e_scalingWnd) =>
                    {
                        RefreshScalingLine(ScalingWindow, ScalingWindow.Slider_Scale.Tag as Element, canvas);
                    };
                    ScalingWindow.Closed += (object sender_scalingWnd, EventArgs e_scalingWnd) =>
                    {
                        canvas.Children.Remove(ScalingLine);
                    };

                    ScalingWindow.Slider_Scale.Tag = element;
                };
        }

        private void InitMenuItem_MakeBackground(MenuItem mi_makeBg, Element element, Canvas canvas)
        {
            mi_makeBg.Header = MENU_ITEM_NAME_MAKE_BACKGROUND;
            mi_makeBg.Click +=
                (object sender, RoutedEventArgs eargs) =>
                {
                    if (element.ZIndex == _minZindex)
                    {
                        List<Element> allElements = _bufferUndoRedo.GetCurrentElements();
                        if (allElements.FirstOrDefault(e => e != element && e.ZIndex == element.ZIndex) != null)
                        {
                            _minZindex--;

                            element.ZIndex = _minZindex;

                            if (element is CompositeElement)
                            {
                                if ((element as CompositeElement).RelativeElement != null)
                                    (element as CompositeElement).RelativeElement.ZIndex = element.ZIndex;
                            }
                        }
                    }
                    else
                    {
                        _minZindex--;

                        element.ZIndex = _minZindex;

                        if (element is CompositeElement)
                        {
                            if ((element as CompositeElement).RelativeElement != null)
                                (element as CompositeElement).RelativeElement.ZIndex = element.ZIndex;
                        }
                    }

                };
        }

        private void InitMenuItem_MakeForeground(MenuItem mi_makeFg, Element element, Canvas canvas)
        {
            mi_makeFg.Header = MENU_ITEM_NAME_MAKE_FOREGROUND;
            mi_makeFg.Click +=
                (object sender, RoutedEventArgs eargs) =>
                {
                    if (element.ZIndex == _maxZindex)
                    {
                        List<Element> allElements = _bufferUndoRedo.GetCurrentElements();
                        var lol = (from a
                                       in allElements
                                   select a.ZIndex);
                        //List<int> lol = allElements.ToDictionary<int>(e => e.ZIndex).ToArray();
                        if (allElements.FirstOrDefault(e => e != element && e.ZIndex == element.ZIndex) != null)
                        {
                            _maxZindex++;

                            element.ZIndex = _maxZindex;

                            if (element is CompositeElement)
                            {
                                if ((element as CompositeElement).RelativeElement != null)
                                    (element as CompositeElement).RelativeElement.ZIndex = element.ZIndex;
                            }
                        }
                    }
                    else
                    {
                        _maxZindex++;

                        element.ZIndex = _maxZindex;

                        if (element is CompositeElement)
                        {
                            if ((element as CompositeElement).RelativeElement != null)
                                (element as CompositeElement).RelativeElement.ZIndex = element.ZIndex;
                        }
                    }

                };
        }

        private void InitMenuItem_LayerDown(MenuItem mi_layerDown, Element element, Canvas canvas)
        {
            mi_layerDown.Header = MENU_ITEM_NAME_LAYER_DOWN;
            mi_layerDown.Click +=
                (object sender, RoutedEventArgs e) =>
                {
                    element.ZIndex--;

                    if (element is CompositeElement)
                    {
                        if ((element as CompositeElement).RelativeElement != null)
                            (element as CompositeElement).RelativeElement.ZIndex--;
                    }

                    if (element.ZIndex < _minZindex)
                        _minZindex = element.ZIndex;
                };
        }

        private void InitMenuItem_LayerUp(MenuItem mi_layerUp, Element element, Canvas canvas)
        {
            mi_layerUp.Header = MENU_ITEM_NAME_LAYER_UP;
            mi_layerUp.Click +=
                (object sender, RoutedEventArgs e) =>
                {
                    element.ZIndex++;

                    if (element is CompositeElement)
                    {
                        if ((element as CompositeElement).RelativeElement != null)
                            (element as CompositeElement).RelativeElement.ZIndex++;
                    }

                    if (element.ZIndex > _maxZindex)
                        _maxZindex = element.ZIndex;
                };
        }

        private void InitMenuItem_Rotate(MenuItem mi_rotate, Element element, Canvas canvas)
        {
            mi_rotate.Header = MENU_ITEM_NAME_ROTATE;
            mi_rotate.Click +=
                (object sender, RoutedEventArgs e) =>
                {
                    //if (element is CompositeElement == false)
                    //   return;

                    //CompositeElement compElement = element as CompositeElement;

                    if (RotatingWindow != null)
                        RotatingWindow.Close();
                    RotatingWindow = new RotatingWindow();
                    RotatingWindow.Left = this.Left + element.Position.X - RotatingWindow.Width / 2;
                    RotatingWindow.Top = this.Top + element.Position.Y - 50;
                    //RotatingWindow.Title = "Поворот (зуб №" + Convert.ToInt32(Regex.Match(compElement.Source.ToString(), @"\d+").Value) + ")";
                    RotatingWindow.Show();
                    RefreshScalingLine(RotatingWindow, element, canvas);

                    RotatingWindow.Slider_Rotate.ValueChanged += (object sender_slider, RoutedPropertyChangedEventArgs<double> e_slider) =>
                    {
                        RefreshScalingLine(RotatingWindow, RotatingWindow.Slider_Rotate.Tag as Element, canvas);
                    };
                    RotatingWindow.LocationChanged += (object sender_scalingWnd, EventArgs e_scalingWnd) =>
                    {
                        RefreshScalingLine(RotatingWindow, RotatingWindow.Slider_Rotate.Tag as Element, canvas);
                    };
                    RotatingWindow.Closed += (object sender_scalingWnd, EventArgs e_scalingWnd) =>
                    {
                        canvas.Children.Remove(ScalingLine);
                    };

                    RotatingWindow.Slider_Rotate.Tag = element;
                };
        }

        private void InitMenuItem_InsufficientlySealed(MenuItem mi_insufficientlySealed, Element element, Canvas canvas)
        {
            mi_insufficientlySealed.Header = MENU_ITEM_NAME_INSUF_SEALED;
            mi_insufficientlySealed.Click +=
                (object sender, RoutedEventArgs e) =>
                {
                    _bufferUndoRedo.RecordStateBefore(canvas);
                    var composite = element as CompositeElement;

                    if (composite.RootSeal != null)
                        canvas.Children.Remove(composite.RootSeal);

                    composite.RootSeal = new Element();
                    composite.RootSealMoving = true;
                    if (composite.RelativeElement != null)
                        composite.RelativeElement.RootSeal = composite.RootSeal;

                    var toothNumber = composite.GroupName.Substring(5, 2);

                    // Get Image from resource
                    BitmapImage src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = new Uri(@"pack://application:,,,/DantistApp;component/Image/Canaly_bad/" + toothNumber + ".png", UriKind.Absolute);
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.EndInit();



                    composite.RootSeal.Source = src;
                    canvas.Children.Add(composite.RootSeal);
                    composite.RootSeal.Width = composite.RootSeal.Source.Width * 2.5;
                    composite.RootSeal.Height = composite.RootSeal.Source.Height * 2.5;
                    composite.RootSeal.Position = new Point(Canvas.GetLeft(composite) + (composite.ActualWidth - composite.RootSeal.Width) / 2,
                                                    Canvas.GetTop(composite) + Values.RootInfSealYShift[toothNumber]);
                    //Canvas.SetLeft(composite.RootSeal, Canvas.GetLeft(composite) + (composite.ActualWidth - composite.RootSeal.Width) / 2);
                    //Canvas.SetTop(composite.RootSeal, Canvas.GetTop(composite) + Values.RootInfSealYShift[toothNumber]);
                    composite.RootSeal.IsFixed = true;

                    Panel.SetZIndex(composite.RootSeal, 3);

                    // Для пломбы
                    AddMenuItem(composite.RootSeal, MenuItemType.Remove);
                    AddMenuItem(composite.RootSeal, MenuItemType.LayerDown);
                    AddMenuItem(composite.RootSeal, MenuItemType.LayerUp);
                    AddMenuItem(composite.RootSeal, MenuItemType.Fix);
                    AddMenuItem(composite.RootSeal, MenuItemType.Scaling);
                    //AddMenuItem(composite.RootSeal, MenuItemType.Rotate);

                    _bufferUndoRedo.RecordStateAfter(canvas);
                };
        }

        private void InitMenuItem_Sealed(MenuItem mi_insufficientlySealed, Element element, Canvas canvas)
        {
            mi_insufficientlySealed.Header = MENU_ITEM_NAME_SEALED;
            mi_insufficientlySealed.Click +=
                (object sender, RoutedEventArgs e) =>
                {
                    _bufferUndoRedo.RecordStateBefore(canvas);
                    var composite = element as CompositeElement;

                    if (composite.RootSeal != null)
                        canvas.Children.Remove(composite.RootSeal);

                    composite.RootSeal = new Element();
                    composite.RootSealMoving = true;
                    if (composite.RelativeElement != null)
                        composite.RelativeElement.RootSeal = composite.RootSeal;

                    var toothNumber = composite.GroupName.Substring(5, 2);

                    // Get Image from resource
                    BitmapImage src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = new Uri(@"pack://application:,,,/DantistApp;component/Image/Canaly_hv/" + toothNumber + ".png", UriKind.Absolute);
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.EndInit();



                    composite.RootSeal.Source = src;
                    canvas.Children.Add(composite.RootSeal);
                    composite.RootSeal.Width = composite.RootSeal.Source.Width * 2.5;
                    composite.RootSeal.Height = composite.RootSeal.Source.Height * 2.5;
                    composite.RootSeal.Position = new Point(Canvas.GetLeft(composite) + (composite.ActualWidth - composite.RootSeal.Width) / 2,
                                                    Canvas.GetTop(composite) + Values.RootSealYShift[toothNumber]);
                    //Canvas.SetLeft(composite.RootSeal, Canvas.GetLeft(composite) + (composite.ActualWidth - composite.RootSeal.Width) / 2);
                    //Canvas.SetTop(composite.RootSeal, Canvas.GetTop(composite) + Values.RootSealYShift[toothNumber]);
                    composite.RootSeal.IsFixed = true;

                    Panel.SetZIndex(composite.RootSeal, 3);

                    // Для пломбы
                    AddMenuItem(composite.RootSeal, MenuItemType.Remove);
                    AddMenuItem(composite.RootSeal, MenuItemType.LayerDown);
                    AddMenuItem(composite.RootSeal, MenuItemType.LayerUp);
                    AddMenuItem(composite.RootSeal, MenuItemType.Fix);
                    AddMenuItem(composite.RootSeal, MenuItemType.Scaling);
                    //AddMenuItem(composite.RootSeal, MenuItemType.Rotate);

                    _bufferUndoRedo.RecordStateAfter(canvas);
                };
        }

        #endregion MENU_ITEMS
        //==============================================================================================



        private TabItem FindParent_TabItem(FrameworkElement fworkElement)
        {
            fworkElement = fworkElement.Parent as FrameworkElement;
            while (fworkElement is TabItem == false &&
                   fworkElement != null)
            {
                fworkElement = fworkElement.Parent as FrameworkElement;
            }
            return fworkElement as TabItem;
        }


        public void RefreshContextMenu(Element element, Canvas canvas)
        {
            ContextMenu contextMenu = element.ContextMenu;
            if (contextMenu == null)
                return;

            MenuItem mi_fix = FindMenuItem(contextMenu, MENU_ITEM_NAME_FIX);
            MenuItem mi_unfix = FindMenuItem(contextMenu, MENU_ITEM_NAME_UNFIX);
            MenuItem mi_merge = FindMenuItem(contextMenu, MENU_ITEM_NAME_MERGE);
            MenuItem mi_unmerge = FindMenuItem(contextMenu, MENU_ITEM_NAME_UNMERGE);
            if (element.IsFixed && mi_fix != null)
            {
                mi_unfix = new MenuItem();
                InitMenuItem_Unfix(mi_unfix, element, canvas, mi_fix);
                Helpers.ReplaceMenuItem(mi_fix, mi_unfix);
            }
            if (!element.IsFixed && mi_unfix != null)
            {
                mi_fix = new MenuItem();
                InitMenuItem_Fix(mi_fix, element, canvas, mi_unfix);
                Helpers.ReplaceMenuItem(mi_unfix, mi_fix);
            }
            if (element is CompositeElement)
            {
                if ((element as CompositeElement).IsMerged && mi_merge != null)
                {
                    mi_unmerge = new MenuItem();
                    InitMenuItem_Unmerge(mi_unmerge, element, canvas, mi_merge);
                    Helpers.ReplaceMenuItem(mi_merge, mi_unmerge);
                }
                if (!(element as CompositeElement).IsMerged && mi_unmerge != null)
                {
                    mi_merge = new MenuItem();
                    InitMenuItem_Merge(mi_merge, element, canvas, mi_unmerge);
                    Helpers.ReplaceMenuItem(mi_unmerge, mi_merge);
                }
            }
        }


        public MenuItem FindMenuItem(ContextMenu contextMenu, string itemName)
        {
            MenuItem menuItem = (from item in contextMenu.Items.OfType<MenuItem>().DefaultIfEmpty()
                                 where item != null && item.Header == itemName
                                 select item).FirstOrDefault();
            return menuItem;
        }

    }
}
