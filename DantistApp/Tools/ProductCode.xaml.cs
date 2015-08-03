using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace DantistApp.Tools
{
    /// <summary>
    /// Interaction logic for ProductCode.xaml
    /// </summary>
    public partial class ProductCode : Window
    {
        string getpassword;
        string regPath;

        public ProductCode(string passname, string path)
        {
            InitializeComponent();

            getpassword = passname;
            regPath = path;
        }

        public bool passwordEntry(String originalPass, String pass)
        {
            if (originalPass == pass)
            {
                RegistryKey regkey = Registry.CurrentUser;
                regkey = regkey.CreateSubKey(regPath); //path

                if (regkey != null)
                {
                    regkey.SetValue("Password", pass); //Value Name,Value Data
                }
                return true;
            }
            else
                return false;
        }

        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            //if password true then send true			
            bool value = passwordEntry(getpassword, textBox_ProductKey.Text);
            if (value == true)
            {
                MessageBox.Show("Спасибо за активацию!", "Активация", MessageBoxButton.OK, MessageBoxImage.Information);
                //this.Hide();
                this.DialogResult = true;
                //this.Close();
            }
            else
                MessageBox.Show("Ключ не действителен! Введите корректный ключ!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //----------------------------------------------		
        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
