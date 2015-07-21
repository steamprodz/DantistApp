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

namespace DantistApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point _mousePosition;
        PanoramaWindow _panoramaWindow;
        //UndoRedoBuffer_Old _bufferUndoRedo_Old;
        UndoRedoBuffer _bufferUndoRedo;
        List<Element> _selectedElements;
        Element _activeElement;
        Point _previousActiveElementPos;

        public MainWindow()
        {
            InitializeComponent();

            //_bufferUndoRedo_Old = new UndoRedoBuffer_Old();
            _bufferUndoRedo = new UndoRedoBuffer();
            _selectedElements = new List<Element>();

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
            List<Element> GroupElements = Helpers.GetLogicalChildCollection<Element>(this);
            foreach (var item in GroupElements)
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

        private void MenuItem_Service_Click(object sender, RoutedEventArgs e)
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
            //List<Element> removedElements = new List<Element>();
            //BufferAction bufAct = new BufferAction();
            //bufAct.Do += () =>
            //{
            //    int n = canvas_main.Children.Count;
            //    for (int i = n-1; i >= 0; i--)
            //    {
            //        if (canvas_main.Children[i] is Element)
            //        {
            //            removedElements.Add(canvas_main.Children[i] as Element);
            //            canvas_main.Children.RemoveAt(i);
            //        }
            //    }
            //};
            //bufAct.Undo += () =>
            //    {
            //        foreach (var item in removedElements)
            //        {
            //            if (canvas_main.Children.Contains(item) == false)
            //                canvas_main.Children.Add(item as UIElement);
            //        }
            //        removedElements.Clear();
            //    };
            //_bufferUndoRedo_Old.StartAction(bufAct);
            _bufferUndoRedo.RecordStateBefore(canvas_main, true);
            canvas_main.Children.Clear();
            _bufferUndoRedo.RecordStateAfter(canvas_main, true);
        }

        private void Btn_Undo_Click(object sender, RoutedEventArgs e)
        {
            //_bufferUndoRedo_Old.Undo();
            _bufferUndoRedo.Undo();
        }

        private void Btn_Redo_Click(object sender, RoutedEventArgs e)
        {
            //_bufferUndoRedo_Old.Redo();
            _bufferUndoRedo.Redo();
            //label2.Content = Canvas.GetLeft(canvas_main.Children[0]);
            //label3.Content = Canvas.GetTop(canvas_main.Children[0]);
        }

        private void MenuItem_RemoveSelectedElements_Click(object sender, RoutedEventArgs e)
        {
            //List<Element> removedElements = new List<Element>();
            //BufferAction bufAct = new BufferAction();
            //bufAct.Do += () =>
            //{
            //    foreach (var item in _selectedElements)
            //    {
            //        removedElements.Add(item);
            //        canvas_main.Children.Remove(item);
            //    }
            //};
            //bufAct.Undo += () =>
            //{
            //    foreach (var item in removedElements)
            //    {
            //        if (canvas_main.Children.Contains(item) == false)
            //            canvas_main.Children.Add(item as UIElement);
            //    }
            //    removedElements.Clear();
            //};
            //_bufferUndoRedo_Old.StartAction(bufAct);
            _bufferUndoRedo.RecordStateBefore(canvas_main, true);
            foreach (var item in _selectedElements)
            {
                canvas_main.Children.Remove(item);
            }
            _bufferUndoRedo.RecordStateAfter(canvas_main, true);
        }

        private void button_AddReport_Click(object sender, RoutedEventArgs e)
        {
            UserControls.ReportElement reportElement = new UserControls.ReportElement();
            reportElement.MainCanvas = canvas_main;

            stackPanel_Report.Children.Remove(grid_ReportButtons);
            stackPanel_Report.Children.Add(reportElement);
            stackPanel_Report.Children.Add(grid_ReportButtons);
        }


    }



}
