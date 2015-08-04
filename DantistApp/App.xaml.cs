using DantistApp.Tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
            string path = @"Software\D\A\P\R";
            Security security = new Security();
            var trialDays = 30;
            bool logic = security.Algorithm("Routed_Event_Args-are-completely-fucked-up", path, trialDays);

            //if (logic == false)
                
        }
    }
}
