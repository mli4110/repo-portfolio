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

namespace Team_Project
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public static Employee currentEmployee;

        public Main()
        {
            Globals.db = new Database();
            InitializeComponent();
        }

        private void main_btnSales_Click(object sender, RoutedEventArgs e)
        {
            Transaction dlg = new Transaction();
            dlg.ShowDialog();
            if (dlg.DialogResult == true) Refresh();
        }

        private void main_btnStats_Click(object sender, RoutedEventArgs e)
        {
            Stats dlg = new Stats(this);
            dlg.ShowDialog();
        }

        private void main_btnInventory_Click(object sender, RoutedEventArgs e)
        {
            Vehicle dlg = new Vehicle(this);
            dlg.ShowDialog();
        }

        public void Refresh()
        {
            // refresh sales stats
        }

        private void LoginLogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if ( btnLoginLogoutButton.Content.ToString() == "Logout")
            {
                currentEmployee = null;
                lblEmployeeInfo.Content = "";
                grdSalesStats.IsEnabled = false;
                btnLoginLogoutButton.Content = "Login";
                return;
            }

            Login dlg = new Login(this);
            dlg.ShowDialog();

            if (dlg.DialogResult == true)
            {
                lblEmployeeInfo.Content = currentEmployee.Id + " : " + currentEmployee.Name;
                grdSalesStats.IsEnabled = true;
                btnLoginLogoutButton.Content = "Logout";
                if (currentEmployee.Title == "Manager") main_btnOrders.Content = "All Orders";
            }
        }

        private void main_btnOrders_Click(object sender, RoutedEventArgs e)
        {
            EmployeeOrders dlg = new EmployeeOrders(this, currentEmployee);
            dlg.ShowDialog();
        }
    }
}
