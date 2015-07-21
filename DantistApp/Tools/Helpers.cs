using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DantistApp.Tools
{
    public class Helpers
    {
        public static List<T> GetLogicalChildCollection<T>(object parent) where T : DependencyObject
        {
            List<T> logicalCollection = new List<T>();
            GetLogicalChildCollection(parent as DependencyObject, logicalCollection);
            return logicalCollection;
        }
        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }


        public static bool IsRectInRect(Rect rect1, Rect rect2)
        {
            bool b = false;
            Rect checkRect = new Rect(rect2.Location, rect2.Size);
            checkRect.Intersect(rect1);
            try
            {
                if (Enumerable.Range((int)checkRect.Size.Width - 1, 2).Contains((int)rect1.Size.Width) &&
                    Enumerable.Range((int)checkRect.Size.Height - 1, 2).Contains((int)rect1.Size.Height))
                    b = true;
            }
            catch { }

            return b;
        }

    }


}
