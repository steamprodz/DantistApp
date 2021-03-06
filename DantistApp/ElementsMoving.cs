﻿using System;
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

        private void MainCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_activeElement != null)
            {
                _bufferUndoRedo.RecordStateAfter(CanvasMain);
                
                CanvasMain.ReleaseMouseCapture();
                _activeElement = null;
            }
            
        }

        TextBox coordsTextBoxOld, coordsTextBoxNew;

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_activeElement != null && 
                !(_activeElement as Element).IsFixed)
            {
                bool canMove = true;

                var position = e.GetPosition(CanvasMain);
                var offset = position - _mousePosition;
                _mousePosition = position;
                Point destinationPoint = new Point(Canvas.GetLeft(_activeElement) + offset.X,
                                                   Canvas.GetTop(_activeElement) + offset.Y);

                Rect canvasRect = new Rect(CanvasMain.RenderSize);

                //check: canvas contains element
                Rect elementRect = new Rect(destinationPoint.X, destinationPoint.Y,
                                        _activeElement.ActualWidth, _activeElement.ActualHeight);
                if (canvasRect.Contains(elementRect) == false)
                    canMove = false;

                //check: canvas contains relative element
                if (_activeElement is CompositeElement && 
                    (_activeElement as CompositeElement).RelativeElement != null)
                {
                        CompositeElement relativeElement = (_activeElement as CompositeElement).RelativeElement;
                        Rect relativeElementRect = new Rect(relativeElement.Position.X + offset.X, relativeElement.Position.Y + offset.Y,
                                                            relativeElement.ActualWidth, relativeElement.ActualHeight);

                    if (canvasRect.Contains(relativeElementRect) == false)
                        canMove = false;
                }

                //movement
                if (canMove)
                {
                    Point p = new Point(Canvas.GetLeft(_activeElement) + offset.X,
                                        Canvas.GetTop(_activeElement) + offset.Y);
                    (_activeElement as Element).Position = p;

                    if (RotatingWindow != null && RotatingWindow.IsLoaded)
                        RefreshScalingLine(RotatingWindow, _activeElement, CanvasMain);

                    if (_activeElement is CompositeElement)
                    {
                        var activeComposite = _activeElement as CompositeElement;

                        if (ScalingWindow != null && ScalingWindow.IsLoaded)
                            RefreshScalingLine(ScalingWindow, activeComposite, CanvasMain);

                        if (activeComposite.RelativeToothNumber != null)
                        {
                            Canvas.SetLeft(activeComposite.RelativeToothNumber, p.X);
                            Canvas.SetTop(activeComposite.RelativeToothNumber, p.Y);
                        }
                        else if (activeComposite.RelativeElement != null && activeComposite.RelativeElement.RelativeToothNumber != null)
                        {
                            Canvas.SetLeft(activeComposite.RelativeElement.RelativeToothNumber, p.X);
                            Canvas.SetTop(activeComposite.RelativeElement.RelativeToothNumber, p.Y);
                        }
                    }

                    if (coordsTextBoxOld != null)
                    {
                        CanvasMain.Children.Remove(coordsTextBoxOld);
                    }


                    //// Для пломбированного корня
                    //if (_activeElement is CompositeElement && (_activeElement as CompositeElement).RootSeal != null)
                    //{
                    //    var composite = _activeElement as CompositeElement;

                    //    Canvas.SetTop(composite.RootSeal, Canvas.GetTop(composite.RootSeal) + offset.Y);
                    //    Canvas.SetLeft(composite.RootSeal, Canvas.GetLeft(composite.RootSeal) + offset.X);
                    //}

                    //coordsTextBoxNew = new TextBox();
                    ////if (_activeElement is CompositeElement && (_activeElement.Name == "element_top"))
                    //coordsTextBoxNew.Text = string.Format("{0};{1}", p.X, p.Y);
                    //CanvasMain.Children.Add(coordsTextBoxNew);
                    //Canvas.SetLeft(coordsTextBoxNew, p.X + 50);
                    //Canvas.SetTop(coordsTextBoxNew, p.Y + 50);

                    //coordsTextBoxOld = coordsTextBoxNew;

                    //Canvas.SetZIndex(coordsTextBoxNew, 10);
                }
            }
        }



    }
}
