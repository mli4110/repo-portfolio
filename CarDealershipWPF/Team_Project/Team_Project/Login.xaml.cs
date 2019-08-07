using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Team_Project
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        Regex numbers = new Regex("[^0-9]+");

        public Login(Window parent)
        {
            InitializeComponent();
            Owner = parent;
            tbEmployeeId.Focus();
        }

        private void tbEmployeeId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numbers.IsMatch(e.Text);
        }

        private void btnAddNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddNewEmployee dlg = new AddNewEmployee(this);
            dlg.ShowDialog();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (tbEmployeeId.Text == "" || pbPassword.Password == "")
            {
                MessageBox.Show("Invalid ID or Password. ");
                return;
            }

            List<Employee> employeeList = Globals.db.GetEmployeeList();
            long employeeId = long.Parse(tbEmployeeId.Text);
            string pw = EncodePassword(pbPassword.Password);

            foreach(Employee em in employeeList)
            {
                if (employeeId == em.Id)
                {
                    if (pw == em.PW)
                    {
                        Main.currentEmployee = em;
                        DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("Invalid ID or Password. ");
                        return;
                    }
                }
            }
        }

        public string EncodePassword(string password)
        {
            string epw = "";
            try
            {
                byte[] bytes = Encoding.Unicode.GetBytes(password);
                byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
                epw = Convert.ToBase64String(inArray);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return epw;
        }



    }
}
