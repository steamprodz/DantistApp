using System;
using System.Collections;
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
using System.Text.RegularExpressions;
using System.Windows.Media.Effects;

namespace DantistApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point _mousePosition;
        PanoramaWindow _panoramaWindow;
        ScalingWindow ScalingWindow;
        Line ScalingLine;
        UndoRedoBuffer _bufferUndoRedo;
        List<Element> _selectedElements;
        List<CompositeElementShell> _tabSelectedElements;
        Element _activeElement;
        Point _previousActiveElementPos;
        


        public MainWindow()
        {
            InitializeComponent();

            _bufferUndoRedo = new UndoRedoBuffer();
            _selectedElements = new List<Element>();
            _tabSelectedElements = new List<CompositeElementShell>();

            foreach (var item in panel_btns.Children)
            {
                if (item is Button)
                    (item as Button).Click += PanelBtn_Click;
            }

            AddDoubleClickEvents();
        }

        /// <summary>
        /// Adds double-click events for basic elements
        /// </summary>
        private void AddDoubleClickEvents()
        {
            List<Grid> grids = new List<Grid>();
            List<Element> elements = Helpers.GetLogicalChildCollection<Element>(this);
            foreach (var item in elements)
            {
                try
                {
                    (item as Element).MouseLeftButtonDown += Element_AddingOnDoubleClick;
                }
                catch { }
            }

        }

        /// <summary>
        /// Click from right side panel (with text descriptions)
        /// calls bottom tabItem selecting
        /// </summary>
        private void PanelBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            String subStr = btn.Content.ToString().Substring(0, 3);
            int number = Convert.ToInt32(Regex.Match(subStr, @"\d+").Value);

            tabControl_elements.SelectedIndex = number - 1;
        }

        private void menuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow wnd = new SettingsWindow(stackPanel_Report);
            wnd.ShowDialog();
        }

        private void MenuItem_PanoramaWindow_Click(object sender, RoutedEventArgs e)
        {
            if (_panoramaWindow == null || !_panoramaWindow.IsLoaded)
            {
                _panoramaWindow = new PanoramaWindow();
                _panoramaWindow.Show();
            }
            else
                _panoramaWindow.Activate();
        }

        private void MenuItem_RemoveAllElements_Click(object sender, RoutedEventArgs e)
        {
            _bufferUndoRedo.RecordStateBefore(CanvasMain);
            CanvasMain.Children.Clear();
            _bufferUndoRedo.RecordStateAfter(CanvasMain);
        }

        private void commandBinding_Undo_Executed(object sender, RoutedEventArgs e)
        {
            //_bufferUndoRedo_Old.Undo();
            _bufferUndoRedo.Undo();
            foreach (var item in CanvasMain.Children)
            {
                RefreshContextMenu(item as Element, CanvasMain);
            }
        }

        private void commandBinding_Redo_Executed(object sender, RoutedEventArgs e)
        {
            _bufferUndoRedo.Redo();
            foreach (var item in CanvasMain.Children)
            {
                RefreshContextMenu(item as Element, CanvasMain);
            }
        }

        private void MenuItem_RemoveSelectedElements_Click(object sender, RoutedEventArgs e)
        {
            _bufferUndoRedo.RecordStateBefore(CanvasMain);
            foreach (var item in _selectedElements)
            {
                CanvasMain.Children.Remove(item);
            }
            _bufferUndoRedo.RecordStateAfter(CanvasMain);
        }

        private void button_AddReport_Click(object sender, RoutedEventArgs e)
        {
            UserControls.ReportElement reportElement = new UserControls.ReportElement { Width = 190 };
            reportElement.MainCanvas = CanvasMain;

            var buttonContainer = Helpers.CopyObject(button_AddReport.Parent as Grid);
            (buttonContainer.Children[0] as Button).Click += button_AddReport_Click;

            int totalReportElements = stackPanel_Report.Children.Count;

            var elementIndex = stackPanel_Report.Children.IndexOf((sender as Button).Parent as UIElement);

            if (elementIndex == totalReportElements - 1)
            {
                //if (elementIndex == 0)
                //{
                //    stackPanel_Report.Children.RemoveAt(0);
                //}

                stackPanel_Report.Children.Add(reportElement);

                stackPanel_Report.Children.Add(buttonContainer);
            }
            else
            {
                var actionResult = MessageBox.Show("Вы действительно хотите изменить историю болезни?",
                    "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (actionResult == MessageBoxResult.Yes)
                {

                    List<UIElement> shiftedReportElements = new List<UIElement>();

                    for (int i = totalReportElements - 1; i > elementIndex; i--)
                    {
                        // add in backwards order
                        shiftedReportElements.Add(stackPanel_Report.Children[i]);
                        stackPanel_Report.Children.RemoveAt(i);
                    }

                    stackPanel_Report.Children.Add(reportElement);
                    stackPanel_Report.Children.Add(buttonContainer);

                    // add back to panel in backwards order
                    for (int i = shiftedReportElements.Count - 1; i >= 0; i--)
                    {
                        stackPanel_Report.Children.Add(shiftedReportElements[i]);
                    }
                }
            }
        }

        private void button_AddCurrentTeeth_Click(object sender, RoutedEventArgs e)
        {
            var selectedTab = tabControl_elements.SelectedItem as TabItem;

            if ((selectedTab.Content as Grid).Children[0] is TabControl)
            {
                var subTab = (selectedTab.Content as Grid).Children[0] as TabControl;
                selectedTab = subTab.SelectedItem as TabItem;
            }

            var compositeElementShellList = Tools.Helpers.GetLogicalChildCollection<CompositeElementShell>(selectedTab);

            //var compositeElementList = Tools.Helpers.GetLogicalChildCollection<CompositeElement>(compositeElementShellList);


            AddSelectedElementsToCanvas(compositeElementShellList);
        }

        private void AddSelectedElementsToCanvas(List<CompositeElementShell> compositeElementShellList)
        {
            //List<CompositeElement> compositeElementList = new List<CompositeElement>();

            _bufferUndoRedo.RecordStateBefore(CanvasMain);
            foreach (CompositeElementShell compositeElementShell in compositeElementShellList)
            {
                foreach (CompositeElement element in (compositeElementShell.Content as Grid).Children)
                {
                    //compositeElementList.Add(element);
                    AddElementToCanvas(element, false);
                }
            }
            _bufferUndoRedo.RecordStateAfter(CanvasMain);

            //foreach (CompositeElement element in compositeElementList)
            //{
            //    AddElementToCanvas(element);
            //}
        }

        private void tabControl_elements_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(e.Source is CompositeElementShell ||
                    Keyboard.IsKeyDown(Key.LeftShift)))
            {
                TabClearSelection();
            }

            var element = e.Source as CompositeElementShell;
            
            if (element is CompositeElementShell)// && tabControl_elements.CaptureMouse())
            {
                if (Keyboard.IsKeyDown(Key.LeftShift) == false)
                {
                    TabClearSelection();
                }

                if (_tabSelectedElements.Contains(element) && Keyboard.IsKeyDown(Key.LeftShift))
                {
                    TabRemoveFromSelection(element as CompositeElementShell);
                }
                else
                {
                    TabAddToSelection(element as CompositeElementShell);
                }                
            }
        }

        private void TabAddToSelection(CompositeElementShell element)
        {
            //DropShadowEffect glowEffect = new DropShadowEffect()
            //{
            //    ShadowDepth = 0,
            //    Color = Colors.GreenYellow,
            //    Opacity = 1,
            //    BlurRadius = 20
            //};
            //element.Effect = glowEffect;

            element.Effect = Tools.EffectsHelper.CreateGlowEffect(Colors.GreenYellow);
            _tabSelectedElements.Add(element);
        }

        private void TabRemoveFromSelection(CompositeElementShell element)
        {
            element.Effect = null;
            _tabSelectedElements.Remove(element);
        }

        private void TabClearSelection()
        {
            foreach (var item in _tabSelectedElements)
            {
                item.Effect = null;
            }
            _tabSelectedElements.Clear();
        }

        private void Button_AddSelectedTeeth_Click(object sender, RoutedEventArgs e)
        {
            AddSelectedElementsToCanvas(_tabSelectedElements);
        }

        private void Button_RemoveSelectedTeeth_Click(object sender, RoutedEventArgs e)
        {
            List<CompositeElement> removeList = new List<CompositeElement>();

            foreach (CompositeElementShell compositeElementShell in _tabSelectedElements)
            {
                foreach (CompositeElement element in (compositeElementShell.Content as Grid).Children)
                {
                    foreach (var item in CanvasMain.Children)
                    {
                        if (item is CompositeElement)
                        {
                            var canvasElement = item as CompositeElement;

                            if (canvasElement.GroupName == element.GroupName)
                            {
                                removeList.Add(canvasElement);
                            }
                        }
                    }
                }
            }

            foreach (var item in removeList)
            {
                RemoveElement(item, CanvasMain);
            }
        }
        

        private void tabControl_elements_LostFocus(object sender, RoutedEventArgs e)
        {
            TabClearSelection();
        }

        private void menuItem_SaveAsPdf_Click(object sender, RoutedEventArgs e)
        {
            if (stackPanel_Report.Children.Count > 1)
            {
                // Some PDF shit
                const int pdfHeaderHeight = 40;
                const int pdfLeftMargin = 20;
                const int pdfTextPadding = 25;
                const int pdfImagePadding = 10;

                //string pdfFileName = @"D:\test.pdf";

                // Initialize PDF document
                PdfSharp.Pdf.PdfDocument pdfDocument = new PdfSharp.Pdf.PdfDocument();
                // Create page and GFX
                PdfSharp.Pdf.PdfPage pdfPage = pdfDocument.AddPage();
                PdfSharp.Drawing.XGraphics pdfGFX = PdfSharp.Drawing.XGraphics.FromPdfPage(pdfPage);
                PdfSharp.Drawing.XFont pdfFont = new PdfSharp.Drawing.XFont("Vendetta", 20, PdfSharp.Drawing.XFontStyle.Regular);

                int yPos = pdfHeaderHeight;
                int xPos = pdfLeftMargin;

                // Parse data to PDF
                for (int i = 1; i < stackPanel_Report.Children.Count; i += 2)
                {
                    // 2 elements per page
                    if ((i - 1) % 4 == 0 && (i - 1) != 0)
                    {
                        // Create new page and GFX
                        pdfPage = pdfDocument.AddPage();
                        pdfGFX = PdfSharp.Drawing.XGraphics.FromPdfPage(pdfPage);
                        
                        yPos = pdfHeaderHeight;
                        xPos = pdfLeftMargin;
                    }

                    // Add elements to page
                    var selectedItem = stackPanel_Report.Children[i] as UserControls.ReportElement;

                    try
                    {

                        pdfGFX.DrawString(selectedItem.textBox_Comments.Text, pdfFont, PdfSharp.Drawing.XBrushes.Black,
                            new PdfSharp.Drawing.XRect(xPos, yPos, pdfPage.Width, 0));

                        yPos += pdfTextPadding;

                        PdfSharp.Drawing.XImage pdfImage = PdfSharp.Drawing.XImage.FromGdiPlusImage(
                            Tools.ImageHelper.ImageWpfToGDI(selectedItem.image_CanvasScreenshot.Source));
                        pdfGFX.DrawImage(pdfImage, xPos, yPos);

                        yPos += (int)pdfImage.Height + pdfImagePadding;
                    }
                    catch { }
                }

                var pdfFileName = Tools.PdfHelper.SavePdfToFile(pdfDocument);

                //pdfDocument.Save(pdfFileName);
                if (pdfFileName != null)
                    System.Diagnostics.Process.Start(pdfFileName);
                else
                    MessageBox.Show("Не удалось создать PDF документ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("История болезни пуста", "Ошибка печати", MessageBoxButton.OK, MessageBoxImage.Stop);
            
        }

        private void menuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuItem_ShowTeethNumbers_Click(object sender, RoutedEventArgs e)
        {
            List<CompositeElement> compositeElements = new List<CompositeElement>();

            foreach (var item in CanvasMain.Children)
            {
                if (item is CompositeElement)
                {
                    compositeElements.Add(item as CompositeElement);
                }
            }

            foreach (CompositeElement element in compositeElements)
            {
                if (element.RelativeToothNumber == null)
                {
                    Label label = DrawToothNumber(element);
                    if (element.CompositeLocation == CompositeLocation.Bot)
                        label.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    if (element.CompositeLocation == CompositeLocation.Top)
                        element.RelativeToothNumber.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private Label DrawToothNumber(CompositeElement element)
        {
            Label labelToothNumber = new Label();

            var groupNameLength = element.GroupName.Length;
            labelToothNumber.Content = element.GroupName.Substring(groupNameLength - 2);

            element.RelativeToothNumber = labelToothNumber;

            labelToothNumber.IsHitTestVisible = false;
            //labelToothNumber.Foreground = Brushes.AliceBlue;
            labelToothNumber.FontSize = 14;
            labelToothNumber.FontWeight = FontWeights.UltraBold;
            labelToothNumber.Effect = new DropShadowEffect()
                {
                    ShadowDepth=0,
                          Color=Colors.Yellow,
                          Opacity=0.8,
                          BlurRadius=3
                };

            CanvasMain.Children.Add(labelToothNumber);
            Canvas.SetLeft(labelToothNumber, Canvas.GetLeft(element));
            Canvas.SetTop(labelToothNumber, Canvas.GetTop(element));

            return labelToothNumber;
        }

        private void DeleteToothNumber(Label labelToothNumber, CompositeElement element)
        {
            element.RelativeToothNumber = null;
            CanvasMain.Children.Remove(labelToothNumber);
        }

        private void menuItem_HideTeethNumbers_Click(object sender, RoutedEventArgs e)
        {
            List<CompositeElement> compositeElements = new List<CompositeElement>();

            foreach (var item in CanvasMain.Children)
            {
                if (item is CompositeElement)
                {
                    compositeElements.Add(item as CompositeElement);
                }
            }

            foreach (CompositeElement element in compositeElements)
            {
                if (element.RelativeToothNumber != null)
                {
                    element.RelativeToothNumber.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

        private void menuItem_MergeAllTeeth_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in CanvasMain.Children)
            {
                if (item is CompositeElement)
                {
                    CompositeElement compElement = item as CompositeElement;
                    MenuItem contextMenu_merge = FindMenuItem(compElement.ContextMenu, MENU_ITEM_NAME_MERGE);
                    if (contextMenu_merge != null)
                    {
                        contextMenu_merge.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                        _bufferUndoRedo.UndoStack.Pop();
                    }
                }
            }
            
        }

        private void menuItem_UnmergeAllTeeth_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in CanvasMain.Children)
            {
                if (item is CompositeElement)
                {
                    CompositeElement compElement = item as CompositeElement;
                    MenuItem contextMenu_merge = FindMenuItem(compElement.ContextMenu, MENU_ITEM_NAME_UNMERGE);
                    if (contextMenu_merge != null)
                    {
                        contextMenu_merge.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                        _bufferUndoRedo.UndoStack.Pop();
                    }
                }
            }

        }


    }



}
