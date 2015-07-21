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
using DantistApp.Tools;

namespace DantistApp
{
    public partial class MainWindow : Window
    {

        private enum MenuItemType
        {
            Remove,
            Fix,
            Merge
        }

        const string MENU_ITEM_NAME_REMOVE = "Удалить элемент";
        const string MENU_ITEM_NAME_FIX = "Зафиксировать элемент";
        const string MENU_ITEM_NAME_UNFIX = "Отменить фиксацию";
        const string MENU_ITEM_NAME_MERGE = "Объединить элементы";
        const string MENU_ITEM_NAME_UNMERGE = "Разъединить элементы";


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

                    _bufferUndoRedo.RecordStateBefore(canvas_main);

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

                    _bufferUndoRedo.RecordStateAfter(canvas_main);

                };
        }

        private void InitMenuItem_Unfix(MenuItem mi_unfix, Element element, Canvas canvas, MenuItem mi_fix)
        {
            mi_unfix.Header = MENU_ITEM_NAME_UNFIX;
            mi_unfix.Click +=
                (object sender, RoutedEventArgs e) =>
                {
                    _bufferUndoRedo.RecordStateBefore(canvas_main);

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

                    _bufferUndoRedo.RecordStateAfter(canvas_main);

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

                    _bufferUndoRedo.RecordStateBefore(canvas_main);

                    CompositeElement compElement = element as CompositeElement;
                    compElement.IsMerged = true;
                    Helpers.ReplaceMenuItem(mi_merge, mi_unmerge);
                    MenuItem relative_mi_merge = FindMenuItem(compElement.RelativeElement.ContextMenu, MENU_ITEM_NAME_MERGE);
                    if (relative_mi_merge != null)
                    {
                        relative_mi_merge.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                        _bufferUndoRedo.UndoStack.Pop();
                    }

                    _bufferUndoRedo.RecordStateAfter(canvas_main);
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

                    _bufferUndoRedo.RecordStateBefore(canvas_main);

                    CompositeElement compElement = element as CompositeElement;
                    compElement.IsMerged = false;
                    Helpers.ReplaceMenuItem(mi_unmerge, mi_merge);
                    MenuItem relative_mi_unmerge = FindMenuItem(compElement.RelativeElement.ContextMenu, MENU_ITEM_NAME_UNMERGE);
                    if (relative_mi_unmerge != null)
                    {
                        relative_mi_unmerge.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                        _bufferUndoRedo.UndoStack.Pop();
                    }

                    _bufferUndoRedo.RecordStateAfter(canvas_main);
                };
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
