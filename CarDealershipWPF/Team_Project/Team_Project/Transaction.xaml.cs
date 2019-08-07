using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
    /// Interaction logic for Transaction.xaml
    /// </summary>
    public partial class Transaction : Window
    {
        Cust selectedCust;
        Car selectedCar;
        Car tradeInCar;
        public static string OrderDateStr;
        public static string PickupDateStr;

        Regex numbers = new Regex("[^0-9.]+");
        Regex numbersLetters = new Regex("[^A-Za-z0-9 -.]+");
        Regex letters = new Regex("[^A-Za-z -.]+");
        Regex postalCode = new Regex("[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ][-][0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]");

        public Transaction()
        {
            InitializeComponent();
        }

        private void tbCustName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = letters.IsMatch(e.Text);
        }

        private void tbAddress_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numbersLetters.IsMatch(e.Text);
        }

        private void tbPostalCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = postalCode.IsMatch(e.Text);
        }

        private void transaction_btnFindCust_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Customer dlg = new Customer();
                if (dlg.ShowDialog() == true)
                {
                    selectedCust = dlg.SelectedCustomer;

                    tbCustName.Text = selectedCust.Name;
                    tbPhone.Text = selectedCust.PhoneNumber;
                    tbPostalCode.Text = selectedCust.PostalCode;
                    tbAddress.Text = selectedCust.Address;
                    tbEmail.Text = selectedCust.Email;
                    tbDriversLicense.Text = selectedCust.DriversLicense;
                    tbCity.Text = selectedCust.City;
                    combobxProvince.Text = selectedCust.Province;
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(this, ex.Message, "Error ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonSelectCar_Click(object sender, RoutedEventArgs e)
        {
            var form = new Vehicle(this);
            var result = form.ShowDialog();

            if (form.DialogResult == true)
            {
                Car val = form.sc;
                selectedCar = val;

                lblMake.Content = selectedCar.Make;
                lblModel.Content = selectedCar.Model;
                lblOdometer.Content = selectedCar.Odometer;
                lblPrice.Content = selectedCar.Price;
                lblTrans.Content = selectedCar.Trans.ToString();
                lblYear.Content = selectedCar.Year;
                lblColor.Content = selectedCar.Color;
                lblDrivetrain.Content = selectedCar.Drive_Train.ToString();

                // Also display this price in Total-tab
                lblTotalCarPrice.Content = lblPrice.Content;
            }
        }

        private void btnTradeSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long id = Globals.db.GetNextCarId();
                string make = tbTradeMake.Text;
                string model = tbTradeModel.Text;

                int.TryParse(tbTradeYear.Text, out int year);
                string vin = tbTradeVin.Text;
                string color = tbTradeColor.Text;

                string strStyle = combobxTradeStyle.Text;
                Car.StyleEnum style;
                bool isStyle = Enum.TryParse(strStyle, out style);

                string strTrans = combobxTradeTransmission.Text;
                Car.TransmissionEnum trans;
                bool isTrans = Enum.TryParse(strTrans, out trans);

                string strDriveTrain = combobxTradeDriveTrain.Text;
                Car.DriveTrainEnum driveTrain;
                bool isDriveTrain = Enum.TryParse(strDriveTrain, out driveTrain);

                double.TryParse(tbTradePrice.Text, out double price);
                long.TryParse(tbTradeOdometer.Text, out long odometer);

                bool isNew = false;
                bool isSaleable = false;
                if (chkbxTradeSaleable.IsChecked == true) isSaleable = true;

                if (VerifyAllTradeInInputs() == false)
                {
                    return;
                }

                Car c = new Car(id, vin, isNew, make, model, year, style, trans, driveTrain, price, odometer, color, isSaleable);
                Globals.db.AddCar(c);

                tradeInCar = c;
                lblTotalTradeInPrice.Content = c.Price;

                MessageBox.Show("Trade in ID: " + c.Id + " has been successfully saved. ");
                btnTradeSave.IsEnabled = false;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is FormatException)
                    MessageBox.Show("Invalid data. ", "Error ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool VerifyAllTradeInInputs()
        {
                 if (!double.TryParse(tbTradePrice.Text, out double price) || price < 0 || price > 5000000)
                {
                    MessageBox.Show("Price must be a  number, which is between 0 - 5,000,000. ");
                    return false;
                }
                if (!Regex.IsMatch(tbTradeColor.Text, "^[0-9a-zA-Z .]{1,20}$"))
                {
                    MessageBox.Show("Color must contain 1 - 20 characters. ");
                    return false;
                }
                if (!long.TryParse(tbTradeOdometer.Text, out long odometer) || odometer < 0 || odometer > 500000)
                {
                    MessageBox.Show("Odometer must be a number, which is between 0 - 500,000. ");
                    return false;
                }
                if (!Regex.IsMatch(tbTradeVin.Text, "^[0-9a-zA-Z]{17}$"))
                {
                    MessageBox.Show("Vin is invalid! Vin must contain 17 characters. ");
                    return false;
                }
                if (!int.TryParse(tbTradeYear.Text, out int year) || year < 1000 || year > 3000)
                {
                    MessageBox.Show("Year must be a number, which is between 1000 - 3000. ");
                    return false;
                }
                if (!Regex.IsMatch(tbTradeMake.Text, "^[a-zA-Z0-9 .]{1,50}$"))
                {
                    MessageBox.Show("Make must contain 1 - 50 characters. ");
                    return false;
                }
                if (!Regex.IsMatch(tbTradeModel.Text, "^[a-zA-Z0-9 .]{1,50}$"))
                {
                    MessageBox.Show("Model must contain 1 - 50 characters. ");
                    return false;
                }
                if (combobxTradeDriveTrain.Text == "")
                {
                    MessageBox.Show("DriveTrain cannot be empty. ");
                    return false;
                }
                if (combobxTradeStyle.Text == "")
                {
                    MessageBox.Show("Style cannot be empty. ");
                    return false;
                }
                if (combobxTradeTransmission.Text == "")
                {
                    MessageBox.Show("Transmission cannot be empty. ");
                    return false;
                }
            return true;
        }

        private void tbTradePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numbers.IsMatch(e.Text);
        }

        private void btnTradeClear_Click(object sender, RoutedEventArgs e)
        {
            tbTradeColor.Text = "";
            tbTradeMake.Text = "";
            tbTradeModel.Text = "";
            tbTradeOdometer.Text = "";
            tbTradePrice.Text = "";
            tbTradeVin.Text = "";
            tbTradeYear.Text = "";
            chkbxTradeSaleable.IsChecked = false;
            combobxTradeDriveTrain.Text = "";
            combobxTradeStyle.Text = "";
            combobxTradeTransmission.Text = "";
        }

        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCar == null)
            {
                MessageBox.Show("Select a car. ");
                return;
            }

            if (selectedCar.Saleable == false)
            {
                MessageBox.Show("This car has already been sold. ");
                return;
            }

            bool[] extraOptionResult = { cbWarranty.IsChecked.Value, cbWinterTires.IsChecked.Value, cbRustProofing.IsChecked.Value };

            try
            {
                OrderDateStr = dpOrderDate.Text;
                PickupDateStr = dpPickupDate.Text;

                if (selectedCust == null)
                {
                    throw new ArgumentException("Please select a customer!");
                }
                if (selectedCar == null)
                {
                    throw new ArgumentException("Please select a car!");
                }
                if (string.IsNullOrEmpty(OrderDateStr))
                {
                    throw new ArgumentException("Please select Order Date!");
                }
                if (string.IsNullOrEmpty(PickupDateStr))
                {
                    throw new ArgumentException("Please select Pickup Date!");
                }
                if(dpOrderDate.SelectedDate > dpPickupDate.SelectedDate)
                {
                    throw new ArgumentException("Pickup Date cannot be eailier than Order Date!");
                }

                OrderWindow dlg = new OrderWindow(extraOptionResult, selectedCust, selectedCar, tradeInCar);
                dlg.ShowDialog();

                if (dlg.DialogResult == true)
                {
                    EmployeeOrders dlg1 = new EmployeeOrders(this, Main.currentEmployee);
                    dlg1.ShowDialog();
                    if (DialogResult == true) Close();
                }
                if (selectedCar.Saleable == false) Close();
            }
            catch (ArgumentException ex)
            {
                    MessageBox.Show(this, ex.Message, "Input Error!");
                    return;
            }

        }

        // Extra Option Tab - Resovled!!
        private void cbWarranty_Checked(object sender, RoutedEventArgs e)
        {
            string name = cbWarranty.Content.ToString();
            double price = Globals.db.GetExtraOptionPrice(name);
            lblWarranties.Content = price;
            GetExtraOptionSubtotal(price);
        }

        private void cbWinterTires_Checked(object sender, RoutedEventArgs e)
        {
            string name = cbWinterTires.Content.ToString();
            double price = Globals.db.GetExtraOptionPrice(name);
            lblWinterTires.Content = price;
            GetExtraOptionSubtotal(price);
        }

        private void cbRustProofing_Checked(object sender, RoutedEventArgs e)
        {
            string name = cbRustProofing.Content.ToString();
            double price = Globals.db.GetExtraOptionPrice(name);
            lblRustProofing.Content = price;
            GetExtraOptionSubtotal(price);
        }

        double subtotal = 0;

        private void GetExtraOptionSubtotal(double price)
        {
            subtotal += price;
            if (subtotal > 0)
            {
                lblExtraOptionSubtotal.Content = subtotal;
            }
            else
            {
                lblExtraOptionSubtotal.Content = 0;
            }

            lblTotalExtraOption.Content = lblExtraOptionSubtotal.Content; // Also display this price in Total-tab
        }

        private void cbWarranty_Unchecked(object sender, RoutedEventArgs e)
        {
            string name = cbWarranty.Content.ToString();
            double price = -(Globals.db.GetExtraOptionPrice(name));
            lblWarranties.Content = 0;
            GetExtraOptionSubtotal(price);
        }

        private void cbWinterTires_Unchecked(object sender, RoutedEventArgs e)
        {
            string name = cbWinterTires.Content.ToString();
            double price = -(Globals.db.GetExtraOptionPrice(name));
            lblWinterTires.Content = 0;
            GetExtraOptionSubtotal(price);
        }

        private void cbRustProofing_Unchecked(object sender, RoutedEventArgs e)
        {
            string name = cbRustProofing.Content.ToString();
            double price = -(Globals.db.GetExtraOptionPrice(name));
            lblRustProofing.Content = 0;
            GetExtraOptionSubtotal(price);
        }

        // Total-tab get the Grand total
        private void TabItemTotal_Selected(object sender, RoutedEventArgs e)
        {
            double total = 0;
            total = int.Parse(lblTotalCarPrice.Content.ToString()) + int.Parse(lblTotalExtraOption.Content.ToString()) - int.Parse(lblTotalTradeInPrice.Content.ToString());
            lblTotalGrandTotal.Content = total;
        }

    }
}
