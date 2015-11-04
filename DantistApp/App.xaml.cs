using DantistApp.Tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DantistApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string path = @"Software\D\A\P\R\ZZZ";
            Security security = new Security();
            var trialDays = 2;
            bool logic = security.Algorithm("1822ebsjd544d44vss8ds7vs9asdd7a", path, trialDays);

            //if (logic == false)
                
        }
    }
}
