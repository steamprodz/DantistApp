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
            Replace
        }

        const string MENU_ITEM_NAME_REMOVE = "Удалить элемент";
        const string MENU_ITEM_NAME_FIX = "Зафиксировать элемент";
        const string MENU_ITEM_NAME_UNFIX = "Отменить фиксацию";
        const string MENU_ITEM_NAME_MERGE = "Объединить элементы";
        const string MENU_ITEM_NAME_UNMERGE = "Разъединить элементы";
        const string MENU_ITEM_NAME_REPLACE = "Замена";


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
                    contextMenu.Items.Add(mi_fix);
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

                default: break;
            }
            if (element.ContextMenu == null)
                element.ContextMenu = contextMenu;
        }


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
                        MenuItem relative_mi_fix = FindMenuItem(compElement.RelativeElement.ContextMenu, MENU_ITEM_NAME_FIX);
                        if (compElement.IsMerged && relative_mi_fix != null)
                        {
                            relative_mi_fix.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                            _bufferUndoRedo.UndoStack.Pop();
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
                        MenuItem relative_mi_unfix = FindMenuItem(compElement.RelativeElement.ContextMenu, MENU_ITEM_NAME_UNFIX);
                        if (compElement.IsMerged && relative_mi_unfix != null)
                        {
                            relative_mi_unfix.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                            _bufferUndoRedo.UndoStack.Pop();
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
                    MenuItem relative_mi_merge = FindMenuItem(compElement.RelativeElement.ContextMenu, MENU_ITEM_NAME_MERGE);
                    if (relative_mi_merge != null)
                    {
                        relative_mi_merge.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                        _bufferUndoRedo.UndoStack.Pop();
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
                    MenuItem relative_mi_unmerge = FindMenuItem(compElement.RelativeElement.ContextMenu, MENU_ITEM_NAME_UNMERGE);
                    if (relative_mi_unmerge != null)
                    {
                        relative_mi_unmerge.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                        _bufferUndoRedo.UndoStack.Pop();
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
