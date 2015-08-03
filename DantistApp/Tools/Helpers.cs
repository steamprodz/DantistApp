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
using System.Windows.Markup;
using System.Xml;
using System.IO;
using DantistApp.Elements;


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

        public static T CopyObject<T>(T obj)
        {
            string objXaml = XamlWriter.Save(obj);

            StringReader stringReader = new StringReader(objXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            T newObj = (T)XamlReader.Load(xmlReader);

            return newObj;
        }


        public static void ReplaceMenuItem(MenuItem oldItem, MenuItem newItem)
        {
           ItemsControl menu = oldItem.Parent as ItemsControl;
            if (menu != null)
            {
                int index = menu.Items.IndexOf(oldItem);
                menu.Items.Remove(oldItem);
                menu.Items.Insert(index, newItem);
            }
        }

        public static string GetFontPath(string fontName)
        {
            var windir = Environment.GetEnvironmentVariable("windir");
            var path = windir + @"\Fonts\" + fontName;

            return path;
        }
    }


}
