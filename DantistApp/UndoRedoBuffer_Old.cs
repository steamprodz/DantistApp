using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class BufferAction
    {
        public Action Undo;
        public Action Do;
    }

    public class UndoRedoBuffer_Old
    {
        public Stack<BufferAction> UndoStack { get; set; }
        public Stack<BufferAction> RedoStack { get; set; }

        public UndoRedoBuffer_Old()
        {
            this.UndoStack = new Stack<BufferAction>();
            this.RedoStack = new Stack<BufferAction>();
        }
        
        public void StartAction(BufferAction bufferAction)
        {
            bufferAction.Do();
            UndoStack.Push(bufferAction);
            RedoStack.Clear();
        }

        public void Undo()
        {
            if (UndoStack.Any())
            {
                BufferAction bufferAction = UndoStack.Pop();
                bufferAction.Undo();
                RedoStack.Push(bufferAction);
            }
        }

        public void Redo()
        {
            if (RedoStack.Any())
            {
                BufferAction bufferAction = RedoStack.Pop();
                bufferAction.Do();
                UndoStack.Push(bufferAction);
            }
        }
    }


}