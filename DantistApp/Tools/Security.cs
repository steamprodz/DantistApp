using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DantistApp.Tools
{
    class Security
    {
        static private  int MaxDays;

        private  string GlobalPath;

        private  void FirstTime()
        {
            RegistryKey regkey = Registry.CurrentUser;
            regkey = regkey.CreateSubKey(GlobalPath); //path

            DateTime dt = DateTime.Now;
            string onlyDate = dt.ToShortDateString(); // get only date not time

            regkey.SetValue("Install", onlyDate); //Value Name,Value Data
            regkey.SetValue("Use", onlyDate); //Value Name,Value Data
        }

        private  String CheckfirstDate()
        {
            RegistryKey regkey = Registry.CurrentUser;
            regkey = regkey.CreateSubKey(GlobalPath); //path
            string Br = (string)regkey.GetValue("Install");
            if (regkey.GetValue("Install") == null)
                return "First";
            else
                return Br;
        }

        private  bool CheckPassword(String pass)
        {
            RegistryKey regkey = Registry.CurrentUser;
            regkey = regkey.CreateSubKey(GlobalPath); //path
            string Br = (string)regkey.GetValue("Password");
            if (Br == pass)
                return true; //good
            else
                return false;//bad
        }

        private  String DayDifPutPresent()
        {
            // get present date from system
            DateTime dt = DateTime.Now;
            string today = dt.ToShortDateString();
            DateTime presentDate = Convert.ToDateTime(today);

            // get instalation date
            RegistryKey regkey = Registry.CurrentUser;
            regkey = regkey.CreateSubKey(GlobalPath); //path
            string Br = (string)regkey.GetValue("Install");
            DateTime installationDate = Convert.ToDateTime(Br);

            TimeSpan diff = presentDate.Subtract(installationDate); //first.Subtract(second);
            int totaldays = (int)diff.TotalDays;

            // special check if user chenge date in system
            string usd = (string)regkey.GetValue("Use");
            DateTime lastUse = Convert.ToDateTime(usd);
            TimeSpan diff1 = presentDate.Subtract(lastUse); //first.Subtract(second);
            int useBetween = (int)diff1.TotalDays;

            // put next use day in registry
            regkey.SetValue("Use", today); //Value Name,Value Data

            if (useBetween >= 0)
            {

                if (totaldays < 0)
                    return "Error"; // if user change date in system like date set before installation
                else if (totaldays >= 0 && totaldays <= MaxDays)
                    return Convert.ToString(MaxDays - totaldays); //how many days remaining
                else
                    return "Expired"; //Expired
            }
            else
                return "Error"; // if user change date in system
        }

        private  void BlackList()
        {
            RegistryKey regkey = Registry.CurrentUser;
            regkey = regkey.CreateSubKey(GlobalPath); //path

            regkey.SetValue("Black", "True");

        }

        private  bool BlackListCheck()
        {
            RegistryKey regkey = Registry.CurrentUser;
            regkey = regkey.CreateSubKey(GlobalPath); //path
            string Br = (string)regkey.GetValue("Black");
            if (regkey.GetValue("Black") == null)
                return false; //No
            else
                return true;//Yes
        }

        public  bool Algorithm(String appPassword, String pass, int trialDays)
        {
            MaxDays = trialDays;
            GlobalPath = pass;
            bool chpass = CheckPassword(appPassword);
            if (chpass == true) //execute
                return true;
            else
            {
                bool block = BlackListCheck();
                if (block == false)
                {
                    string chinstall = CheckfirstDate();
                    if (chinstall == "First")
                    {
                        FirstTime();// installation date
                        var ds = MessageBox.Show("Вы используете триальную версию! Хотите активировать ее прямо сейчас?",
                            "Код продукта", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        if (ds == MessageBoxResult.Yes)
                        {
                            ProductCode f1 = new ProductCode(appPassword, GlobalPath);
                            var ds1 = f1.ShowDialog();
                            if (ds1 == true)
                                return true;
                            else
                                return false;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        string status = DayDifPutPresent();
                        if (status == "Error")
                        {
                            BlackList();
                            var ds = MessageBox.Show("Приложение не может быть запущено, была попытка изменения даты! Активируйте, чтобы продолжить.",
                                "Terminate Error-02", MessageBoxButton.OK, MessageBoxImage.Error);
                            if (ds == MessageBoxResult.OK)
                            {
                                ProductCode f1 = new ProductCode(appPassword, GlobalPath);
                                var ds1 = f1.ShowDialog();
                                if (ds1 == true)
                                    return true;
                                else
                                    return false;
                            }
                            else
                                return false;
                        }
                        else if (status == "Expired")
                        {
                            var ds = MessageBox.Show("Время работы приложения истекло! Активируйте, чтобы продолжить.",
                                "Код продукта", MessageBoxButton.OK, MessageBoxImage.Information);
                            if (ds == MessageBoxResult.OK)
                            {
                                ProductCode f1 = new ProductCode(appPassword, GlobalPath);
                                var ds1 = f1.ShowDialog();
                                if (ds1 == true)
                                    return true;
                                else
                                    return false;
                            }
                            else
                                return false;
                        }
                        else // execute with how many day remaining
                        {
                            var ds = MessageBox.Show("Вы используете триальную версию, у Вас осталось " + status + " дней! Хотите ли Вы выполнить активацию сейчас?",
                                "Код продукта", MessageBoxButton.YesNo, MessageBoxImage.Information);
                            if (ds == MessageBoxResult.Yes)
                            {
                                ProductCode f1 = new ProductCode(appPassword, GlobalPath);
                                var ds1 = f1.ShowDialog();
                                if (ds1 == true)
                                    return true;
                                else
                                    return false;
                            }
                            else
                                return true;
                        }
                    }
                }
                else
                {
                    var ds = MessageBox.Show("Приложение не может быть запущено, была попытка изменения даты! Активируйте, чтобы продолжить.",
                        "Terminate Error-01", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (ds == MessageBoxResult.OK)
                    {
                        ProductCode f1 = new ProductCode(appPassword, GlobalPath);
                        var ds1 = f1.ShowDialog();
                        if (ds1 == true)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                    //return "BlackList";
                }
            }
        }
    }
}
