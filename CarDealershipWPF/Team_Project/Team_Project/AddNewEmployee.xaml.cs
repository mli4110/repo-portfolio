using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace Team_Project
{
    /// <summary>
    /// Interaction logic for AddNewEmployee.xaml
    /// </summary>
    /// 
    
    public class Employee
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string PW { get; set; }

        public Employee(long id, string name, string title, string pw)
        {
            Id = id;
            Name = name;
            Title = title;
            PW = pw;
        }
        
        public Employee()
        {

        }

        public override string ToString()
        {
            return string.Format("{0} : {1} - {2}", Id, Name, Title);
        }
    }
    
    public partial class AddNewEmployee : Window
    {
        Employee emToUpdate;
        Regex Letters = new Regex("[^A-Za-z -.]+");

        public AddNewEmployee(Window parent)
        {
            InitializeComponent();
            Owner = parent;
            AddEmployeeDlg_tbStatusBar.Text = "";
            lblEmployeeId.Content = Globals.db.GetNextEmployeeId();
            RefreshEmployeeList();
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

        private void btnSaveNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            CheckInputs();
            try
            {
                if (emToUpdate != null)
                {
                    if (VerifyOldPassword() == false) return;
                    emToUpdate.Name = tbEmployeeName.Text;
                    emToUpdate.Title = tbEmployeeTitle.Text;
                    string pw = EncodePassword(pbCreatePassword.Password.ToString());
                    emToUpdate.PW = pw;
                    Globals.db.UpdateEmployee(emToUpdate);
                    RefreshEmployeeList();
                    AddEmployeeDlg_tbStatusBar.Text = "Employee ID: " + emToUpdate.Id + " had been updated. ";
                    Clear();
                }
                else
                {
                    long employeeId = (long)lblEmployeeId.Content;
                    string employeeName = tbEmployeeName.Text;
                    string employeeTitle = tbEmployeeTitle.Text;
                    string pw = EncodePassword(pbCreatePassword.Password.ToString());
                    Employee em = new Employee(employeeId, employeeName, employeeTitle, pw);
                    Globals.db.AddEmployee(em);
                    RefreshEmployeeList();
                    AddEmployeeDlg_tbStatusBar.Text = "Employee ID: " + em.Id + " had been saved. ";
                    Clear();
                }
            }
            catch (Exception ex)
            {
                if (ex is SqlException) MessageBox.Show(this, ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool VerifyOldPassword()
        {
            bool isValid = false;
            string oldPw = EncodePassword(pbConfirmOldPasswordOnChange.Password.ToString());
            if (emToUpdate.PW != oldPw)
            {
                MessageBox.Show(this, "Error confirming old password: ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }

        public void CheckInputs()
        {
            string p1 = pbCreatePassword.Password.ToString();
            string p2 = pbConfirmPassword.Password.ToString();
            if (p1 != p2)
            {
                MessageBox.Show(this, "Password inputs do not match ", "Password Error ", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (tbEmployeeName.Text.Length < 2 || tbEmployeeName.Text.Length > 30)
            {
                MessageBox.Show(this, "Employee name must be between 2 and 30 characters long. ", "Name Error ", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (tbEmployeeTitle.Text == "")
            {
                MessageBox.Show(this, "Employee title cannot be empty. ", "Title Error ", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        public void RefreshEmployeeList()
        {
            List<Employee> employeeList = Globals.db.GetEmployeeList();
            lvEmployee.ItemsSource = employeeList;
        }

        public void Clear()
        {
            lblEmployeeId.Content = Globals.db.GetNextEmployeeId();
            tbEmployeeName.Text = "";
            tbEmployeeTitle.Text = "";
            pbCreatePassword.Password = "";
            pbConfirmPassword.Password = "";
            emToUpdate = null;
            btnSaveNewEmployee.Content = "Save";
            lblCreatePassword.Content = "Create Password:";
            AddEmployeeDlg_tbStatusBar.Text = "";
            pbConfirmOldPasswordOnChange.Password = "";

            pbConfirmOldPasswordOnChange.Visibility = Visibility.Hidden;
            lblConfirmOldPW.Visibility = Visibility.Hidden;
            lblConfirmPW.Content = "Confirm Password:";
        }

        private void AddNewEmployeeDlg_btnClear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void lvEmployee_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvEmployee.SelectedItem != null)
            {
                Clear();
                emToUpdate = lvEmployee.SelectedItem as Employee;
                lblEmployeeId.Content = emToUpdate.Id;
                lblCreatePassword.Content = "Create New Password:";
                tbEmployeeName.Text = emToUpdate.Name;
                tbEmployeeTitle.Text = emToUpdate.Title;
                AddEmployeeDlg_tbStatusBar.Text = "Employee ID: " + emToUpdate.Id + " had been selected. ";
                btnSaveNewEmployee.Content = "Update";

                lblConfirmPW.Content = "Confirm New Password:";
                pbConfirmOldPasswordOnChange.Visibility = Visibility.Visible;
                lblConfirmOldPW.Visibility = Visibility.Visible;
            }
        }

        private void AddNewEmployeeDlg_btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void tbEmployeeName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Letters.IsMatch(e.Text);
        }


    } // MW
} // NS
