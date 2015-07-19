using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;

namespace DantistApp.Tools
{
    public static class ObjectCopier
    {
        public static T CopyObject<T>(T obj)
        {
            string objXaml = XamlWriter.Save(obj);

            StringReader stringReader = new StringReader(objXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            T newObj = (T)XamlReader.Load(xmlReader);

            return newObj;
        }

    }
}
