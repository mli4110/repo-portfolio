using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        Cust selectedCust;
        Car selectedCar;
        Car tradeInCar;
        bool[] extraOption;

        public OrderWindow(bool[] _extraOption, Cust _cust = null, Car _car = null, Car _tradeInCar = null)
        {
            InitializeComponent();

            // Employee
            orderWindow_lblEmpId.Content = Main.currentEmployee.Id;
            orderWindow_lblEmpName.Content = Main.currentEmployee.Name;

            // Customer
            selectedCust = _cust;
            if (selectedCust != null)
            {
                orderWindow_lblCustId.Content = selectedCust.Id;
                orderWindow_lblCustName.Content = selectedCust.Name;
                orderWindow_lblCustPhone.Content = selectedCust.PhoneNumber;
                orderWindow_lblCustDriverLicense.Content = selectedCust.DriversLicense;
                orderWindow_lblCustAddress.Content = selectedCust.Address;
                orderWindow_lblCustCity.Content = selectedCust.City;
                orderWindow_lblCustPostalCode.Content = selectedCust.PostalCode;
            }

            // Car
            selectedCar = _car;
            if (selectedCar != null)
            {
                orderWindow_lblCarId.Content = selectedCar.Id;
                orderWindow_lblCarPrice.Content = selectedCar.Price;
                orderWindow_lblColor.Content = selectedCar.Color;
                orderWindow_lblDrivetrain.Content = selectedCar.Drive_Train;
                orderWindow_lblMake.Content = selectedCar.Make;
                orderWindow_lblModel.Content = selectedCar.Model;
                orderWindow_lblOdometer.Content = selectedCar.Odometer;
                orderWindow_lblTransmission.Content = selectedCar.Trans;
                orderWindow_lblVin.Content = selectedCar.VIN;
                orderWindow_lblYear.Content = selectedCar.Year;
                if (selectedCar.New == true)
                {
                    orderWindow_rbgNew.IsChecked = true;
                }
                else
                {
                    orderWindow_rbgUsed.IsChecked = true;
                }
            }

            // TradeIn Car
            tradeInCar = _tradeInCar;
            if (tradeInCar != null)
            {
                orderWindow_lblTradeInCarId.Content = tradeInCar.Id;
                orderWindow_lblTradeInCarMake.Content = tradeInCar.Make;
                orderWindow_lblTradeInCarModel.Content = tradeInCar.Model;
                orderWindow_lblTradeInCarPrice.Content = tradeInCar.Price;
                orderWindow_lblTradeInCarVin.Content = tradeInCar.VIN;
                orderWindow_lblTradeInCarYear.Content = tradeInCar.Year;
            }
            else
            {
                orderWindow_lblTradeInCarPrice.Content = 0;
            }

            // Extra Option
            extraOption = _extraOption;
            orderWindow_chbWarranty.IsChecked = _extraOption[0];
            orderWindow_chbWinterTire.IsChecked = _extraOption[1];
            orderWindow_chbRustProofing.IsChecked = _extraOption[2];
            string name;
            double price1, price2, price3, extraOptionSubtotal;
            if (orderWindow_chbWarranty.IsChecked.Value)
            {
                name = orderWindow_chbWarranty.Content.ToString();
                price1 = Globals.db.GetExtraOptionPrice(name);
            }
            else
            {
                price1 = 0;
            }
            orderWindow_lblWarrantyPrice.Content = price1;

            if (orderWindow_chbWinterTire.IsChecked.Value)
            {
                name = orderWindow_chbWinterTire.Content.ToString();
                price2 = Globals.db.GetExtraOptionPrice(name);
            }
            else
            {
                price2 = 0;
            }
            orderWindow_lblWinterTirePrice.Content = price2;

            if (orderWindow_chbRustProofing.IsChecked.Value)
            {
                name = orderWindow_chbRustProofing.Content.ToString();
                price3 = Globals.db.GetExtraOptionPrice(name);
            }
            else
            {
                price3 = 0;
            }
            orderWindow_lblRustProofingPrice.Content = price3;
            extraOptionSubtotal = price1 + price2 + price3;
            orderWindow_lblSubtotal.Content = extraOptionSubtotal;

            // Grand Total calculation
            orderWindow_lblTotalPrice.Content = selectedCar.Price + extraOptionSubtotal - double.Parse(orderWindow_lblTradeInCarPrice.Content.ToString());

            //
            orderWindow_lblOrderDate.Content = Transaction.OrderDateStr;
            orderWindow_lblPickupDate.Content = Transaction.PickupDateStr;
        }

        private void orderWindow_btCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            long customerId = selectedCust.Id;
            long empId = Main.currentEmployee.Id;
            long carId = selectedCar.Id;
            DateTime orderDate = DateTime.Parse(Transaction.OrderDateStr);
            DateTime pickupDate = DateTime.Parse(Transaction.PickupDateStr);
            bool warranty = extraOption[0];
            bool winterTire = extraOption[1];
            bool rustProofing = extraOption[2];
            double total = double.Parse(orderWindow_lblTotalPrice.Content.ToString());

            try
            {
                Orders order = new Orders()
                {
                    CustId = customerId,
                    EmployeeId = empId,
                    CarId = carId,
                    OrderDate = orderDate,
                    PickupDate = pickupDate,
                    Warranty = warranty,
                    WinterTires = winterTire,
                    Rustproofing = rustProofing,
                    Total = total
                };

                Globals.db.AddOrder(order);
                selectedCar.Saleable = false;
                Globals.db.UpdateCar(selectedCar);

                MessageBox.Show("Order processed. ");
                long OrderId = Globals.db.GetCurrentMaxOrderId();
                orderWindow_lblOrderId.Content = OrderId;
                orderWindow_btCreateOrder.IsEnabled = false;
                orderWindow_btCancel.Content = "Close";
            }
            catch (SqlException ex)
            {
                MessageBox.Show(this, ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void orderWindow_btPrint_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            orderWindow_btCreateOrder.Visibility = flag ? Visibility.Hidden : Visibility.Visible;
            orderWindow_btPrint.Visibility = flag ? Visibility.Hidden : Visibility.Visible;
            orderWindow_btCancel.Visibility = flag ? Visibility.Hidden : Visibility.Visible;

            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == true)
            {
                //get selected printer capabilities
                System.Printing.PrintCapabilities capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);

                //get scale of the print wrt to screen of WPF visual
                double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / this.ActualWidth, capabilities.PageImageableArea.ExtentHeight / this.ActualHeight);

                //Transform the Visual to scale
                this.LayoutTransform = new ScaleTransform(scale, scale);

                //get the size of the printer page
                Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

                //update the layout of the visual to the printer page size.
                this.Measure(sz);
                this.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

                //now print the visual to printer to fit on the one page.
                printDlg.PrintVisual(this, "First Fit to Page WPF Print");
            }

            flag = false;
            orderWindow_btCreateOrder.Visibility = flag ? Visibility.Hidden : Visibility.Visible;
            orderWindow_btPrint.Visibility = flag ? Visibility.Hidden : Visibility.Visible;
            orderWindow_btCancel.Visibility = flag ? Visibility.Hidden : Visibility.Visible;

            DialogResult = false;
        }
    }
}
