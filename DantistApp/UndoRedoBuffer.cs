using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class CanvasState
    {
        public Canvas Canvas;
        public List<Element> Elements;
        public List<Point> Positions;
        public List<bool> IsFixedFlags;
        public List<bool> IsMergedFlags;
        public bool CountChanged;

        public CanvasState(Canvas canvas, bool countChanged)
        {
            Canvas = canvas;
            Elements = new List<Element>();
            Positions = new List<Point>();
            IsMergedFlags = new List<bool>();
            IsFixedFlags = new List<bool>();
            foreach (var item in canvas.Children)
            {
                if (item is Element)
                {
                    Elements.Add(item as Element);
                    Positions.Add((item as Element).Position);
                    IsFixedFlags.Add((item as Element).IsFixed);
                    if (item is CompositeElement)
                        IsMergedFlags.Add((item as CompositeElement).IsMerged);
                    else
                        IsMergedFlags.Add(false);
                }
            }
            CountChanged = countChanged;
        }
    }

    public class BufferState
    {
        public CanvasState Before;
        public CanvasState After;
    }

    public class UndoRedoBuffer
    {
        public Stack<BufferState> UndoStack { get; set; }
        public Stack<BufferState> RedoStack { get; set; }

        public UndoRedoBuffer()
        {
            this.UndoStack = new Stack<BufferState>();
            this.RedoStack = new Stack<BufferState>();
        }

        public void RecordStateBefore(Canvas canvas, bool countChanged)
        {
            CanvasState canvasState = new CanvasState(canvas, countChanged);
            
            UndoStack.Push(new BufferState());
            UndoStack.Peek().Before = canvasState;
            RedoStack.Clear();
        }

        public void RecordStateAfter(Canvas canvas, bool countChanged)
        {
            CanvasState canvasState = new CanvasState(canvas, countChanged);

            UndoStack.Peek().After = canvasState;
        }


        public void Undo()
        {
            if (UndoStack.Any())
            {
                BufferState bufferState = UndoStack.Pop();
                CanvasState bufferStateBefore = bufferState.Before;
                //if (bufferState.CountChanged)
                //{
                //    bufferState.Canvas.Children.Clear();
                //    foreach (var item in bufferState.Elements)
                //    {
                //        bufferState.Canvas.Children.Add(item);    
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < bufferState.Elements.Count; i++)
                //    {
                //        Element element = bufferState.Canvas.Children[i] as Element;
                //        Element undoElement = bufferState.Elements[i] as Element;
                //        element.Position = bufferState.Positions[i];
                //        element.IsFixed = bufferState.IsFixedFlags[i];
                //        if (element is CompositeElement)
                //            (element as CompositeElement).IsMerged = bufferState.IsMergedFlags[i]; ;
                //    }
                //}

                bufferStateBefore.Canvas.Children.Clear();
                foreach (var item in bufferStateBefore.Elements)
                {
                    bufferStateBefore.Canvas.Children.Add(item);
                }
                for (int i = 0; i < bufferStateBefore.Elements.Count; i++)
                {
                    Element element = bufferStateBefore.Canvas.Children[i] as Element;
                    Element undoElement = bufferStateBefore.Elements[i] as Element;
                    element.Position = bufferStateBefore.Positions[i];
                    element.IsFixed = bufferStateBefore.IsFixedFlags[i];
                    if (element is CompositeElement)
                        (element as CompositeElement).IsMerged = bufferStateBefore.IsMergedFlags[i];
                }

                RedoStack.Push(bufferState);
            }
        }

        public void Redo()
        {
            if (RedoStack.Any())
            {
                BufferState bufferState = RedoStack.Pop();
                CanvasState bufferStateAfter = bufferState.After;
                //if (bufferState.CountChanged)
                //{
                //    bufferState.Canvas.Children.Clear();
                //    foreach (var item in bufferState.Elements)
                //    {
                //        bufferState.Canvas.Children.Add(item);
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < bufferState.Elements.Count; i++)
                //    {
                //        Element element = bufferState.Canvas.Children[i] as Element;
                //        Element undoElement = bufferState.Elements[i] as Element;
                //        element.Position = bufferState.Positions[i];
                //        element.IsFixed = bufferState.IsFixedFlags[i];
                //        if (element is CompositeElement) 
                //            (element as CompositeElement).IsMerged = bufferState.IsMergedFlags[i];
                //    }
                //}


                if (bufferStateAfter == null)
                    return;

                bufferStateAfter.Canvas.Children.Clear();
                foreach (var item in bufferStateAfter.Elements)
                {
                    bufferStateAfter.Canvas.Children.Add(item);
                }
                for (int i = 0; i < bufferStateAfter.Elements.Count; i++)
                {
                    Element element = bufferStateAfter.Canvas.Children[i] as Element;
                    Element undoElement = bufferStateAfter.Elements[i] as Element;
                    element.Position = bufferStateAfter.Positions[i];
                    element.IsFixed = bufferStateAfter.IsFixedFlags[i];
                    if (element is CompositeElement)
                        (element as CompositeElement).IsMerged = bufferStateAfter.IsMergedFlags[i];
                }


                UndoStack.Push(bufferState);
            }
        }

    }
}
