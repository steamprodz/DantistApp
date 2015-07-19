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

namespace DantistApp
{
    public abstract class UndoRedoBuffer<T>
    {
        /// <summary>
        /// Adds an Event to Stack
        /// </summary>
        /// <param name="undoOperation"></param>
        protected abstract void Add(Action<T> undoOperation);

        /// <summary>
        /// Returns the size of the stack
        /// </summary>
        /// <returns></returns>
        public abstract int Size();
        /// <summary>
        /// Holds all the Events in a stack
        /// </summary>
        public Stack<Action> MyUndoOperations { get; set; }
        public Stack<Action> MyRedoOperations { get; set; }

        /// <summary>
        /// Preforms the undo action. Pops the last event and gets a reference to the next event 
        /// and fires the event 
        /// </summary>
        public abstract void Undo();
        /// <summary>
        /// Preforms the Redo action.
        /// </summary>
        public abstract void Redo();       

    }






    //public class Buffer : UndoRedoBuffer<Buffer>
    //{

    //    /// <summary>
    //    /// Returns the size of the Action Event Stack
    //    /// </summary>
    //    /// <returns>Size of this.MyOperations</returns>
    //    public override int Size()
    //    {
    //        return this.MyUndoOperations.Count();
    //    }

    //    public Buffer()
    //    {

    //        this.MyUndoOperations = new Stack<Action>();
    //        this.MyRedoOperations = new Stack<Action>();
    //    }
    //    /// <summary>
    //    /// Pushes the last Action event on the Stack
    //    /// </summary>
    //    /// <param name="undoOperation">Last Action Event</param>
    //    public override void Add(Action undoOperation)
    //    {
    //        //this.MyRedoOperations.Clear();
    //        this.MyUndoOperations.Push(undoOperation);
    //    }

    //    /// <summary>
    //    /// Preforms the undo action. Pops the last event and gets a reference to the next event 
    //    /// and fires the event 
    //    /// </summary>
    //    public override void Undo()
    //    {

    //        if (this.MyUndoOperations != null && this.MyUndoOperations.Any())
    //        {

    //            Action topAction = this.MyUndoOperations.Pop();// remove the very last event.
    //            this.MyRedoOperations.Push(topAction); // add to the redo stack

    //            if (this.MyUndoOperations.Any())
    //            {
    //                // get a reference (peek) to the last event
    //                Action lastAction = this.MyUndoOperations.Peek();
    //                lastAction();// fire event  
    //                //this.Add(lastAction); // add to the undo stack
    //            }

    //        }

    //    }

    //    public override void Redo()
    //    {
    //        if (this.MyRedoOperations.Any())
    //        {

    //            Action lastAction = this.MyRedoOperations.Pop();
    //            lastAction();// fire event
    //            this.Add(lastAction);
    //        }

    //    }
    //}


}