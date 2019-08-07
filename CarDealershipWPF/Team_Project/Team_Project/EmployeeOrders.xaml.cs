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
    /// Interaction logic for EmployeeOrders.xaml
    /// </summary>
    public partial class EmployeeOrders : Window
    {
        Employee em;
        List<Orders> empList = new List<Orders>();

        public EmployeeOrders(Window parent, Employee curr)
        {
            InitializeComponent();
            Owner = parent;
            em = curr;
            GetEmpOrders();
            lvEmployeeOrders.ItemsSource = empList;
            lblEmployeeID.Content = em.Id;
            lblEmployeeName.Content = em.Name;

            if (em.Title == "Manager") rbtnSortByEmpId.Visibility = Visibility.Visible;
        }


        public void GetEmpOrders()
        {
            List<Orders> allOrders = Globals.db.GetAllOrders();

            if (em.Title == "Manager" || em.Title == "Supervisor")
            {
                if (empList != null) empList.Clear();
                empList = allOrders;
                return;
            }

            if (empList != null) empList.Clear();

            foreach (Orders or in allOrders)
            {
                if (or.EmployeeId == em.Id)
                {
                    empList.Add(or);
                }
            }
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void rbtnSortByOrderDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Orders> SortedByOrderDate = empList.OrderBy(o => o.OrderDate).ToList();
                lvEmployeeOrders.ItemsSource = SortedByOrderDate;
                tbStatusBar.Text = "Orders were sorted by Order Date. ";
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void rbtnSortByPickupDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Orders> SortedByPickupDate = empList.OrderBy(o => o.PickupDate).ToList();
                lvEmployeeOrders.ItemsSource = SortedByPickupDate;
                tbStatusBar.Text = "Orders were sorted by Pickup Date. ";
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void rbtnSortByOrderID_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Orders> SortedByOrderID = empList.OrderBy(o => o.OrderId).ToList();
                lvEmployeeOrders.ItemsSource = SortedByOrderID;
                tbStatusBar.Text = "Orders were sorted by Order ID. ";
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void rbtnSortByTotal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Orders> SortedByTotal = empList.OrderBy(o => o.Total).ToList();
                lvEmployeeOrders.ItemsSource = SortedByTotal;
                tbStatusBar.Text = "Orders were sorted by Total. ";
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void rbtnSortByEmpId_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Orders> SortedByEmpID = empList.OrderBy(o => o.EmployeeId).ToList();
                lvEmployeeOrders.ItemsSource = SortedByEmpID;
                tbStatusBar.Text = "Orders were sorted by Employee ID. ";
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    } // 
} //
