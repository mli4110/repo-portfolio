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
    /// Interaction logic for AddUpdateCar.xaml
    /// </summary>
    public partial class AddUpdateCar : Window
    {
        Regex numbers = new Regex("[^0-9]+");
        Car selectedCar;

        public AddUpdateCar(Window parent)
        {
            InitializeComponent();
            Owner = parent;
            tbVehicleId.Text = Globals.db.GetNextCarId().ToString();
            lvVehicle.ItemsSource = Globals.db.GetInventory();
        }

        private void VehicleDlg_btnAddNewCar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedCar == null)
                {
                    long id = Globals.db.GetNextCarId();
                    string make = tbMake.Text;
                    string model = tbModel.Text;
                    int year = int.Parse(tbYear.Text);
                    string vin = tbVin.Text;

                    string strStyle = combobxStyle.Text;
                    Car.StyleEnum style;
                    bool isStyle = Enum.TryParse(strStyle, out style);

                    string strTrans = combobxTransmission.Text;
                    Car.TransmissionEnum trans;
                    bool isTrans = Enum.TryParse(strTrans, out trans);

                    string strDriveTrain = combobxDriveTrain.Text;
                    Car.DriveTrainEnum driveTrain;
                    bool isDriveTrain = Enum.TryParse(strDriveTrain, out driveTrain);

                    bool isPrice = double.TryParse(tbPrice.Text, out double price);

                    bool isNew = false;
                    if (rbNewCar.IsChecked == true) isNew = true;

                    bool isOdometer = long.TryParse(VehicleDlg_tbOdometer.Text, out long odometer);

                    string color = tbColor.Text;

                    bool isSaleable = false;
                    if (chkbxSaleable.IsChecked == true) isSaleable = true;

                    if (VerifyAllTradeInInputs() == false)
                    {
                        return;
                    }

                    Car c = new Car(id, vin, isNew, make, model, year, style, trans, driveTrain, price, odometer, color, isSaleable);
                    Globals.db.AddCar(c);
                    Clear();
                    RefreshCars();
                }
                else
                {
                    selectedCar.Make = tbMake.Text;
                    selectedCar.Model = tbModel.Text;
                    selectedCar.Year = int.Parse(tbYear.Text);
                    selectedCar.VIN = tbVin.Text;
                    selectedCar.Color = tbColor.Text;
                    selectedCar.Price = double.Parse(tbPrice.Text);
                    selectedCar.Odometer = long.Parse(VehicleDlg_tbOdometer.Text);
                    selectedCar.Id = long.Parse(tbVehicleId.Text);

                    bool isDriveTrain = Enum.TryParse(combobxDriveTrain.Text, out Car.DriveTrainEnum driveTrain);
                    selectedCar.Drive_Train = driveTrain;
                    bool isStyle = Enum.TryParse(combobxStyle.Text, out Car.StyleEnum style);
                    selectedCar.Style = style;
                    bool isTrans = Enum.TryParse(combobxTransmission.Text, out Car.TransmissionEnum trans);
                    selectedCar.Trans = trans;

                    if (chkbxSaleable.IsChecked == true) selectedCar.Saleable = true;
                    else selectedCar.Saleable = false;

                    Globals.db.UpdateCar(selectedCar);
                    Clear();
                    RefreshCars();
                }
            }
            catch (Exception ex)
            {
                if (ex is SqlException) MessageBox.Show(this, "Database error: " + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool VerifyAllTradeInInputs()
        {
            if (!double.TryParse(tbPrice.Text, out double price) || price < 0 || price > 5000000)
            {
                MessageBox.Show("Price must be a  number, which is between 0 - 5,000,000. ");
                return false;
            }
            if (!Regex.IsMatch(tbColor.Text, "^[0-9a-zA-Z .]{1,20}$"))
            {
                MessageBox.Show("Color must contain 1 - 20 characters. ");
                return false;
            }
            if (!long.TryParse(VehicleDlg_tbOdometer.Text, out long odometer) || odometer < 0 || odometer > 500000)
            {
                MessageBox.Show("Odometer must be a number, which is between 0 - 500,000. ");
                return false;
            }
            if (!Regex.IsMatch(tbVin.Text, "^[0-9a-zA-Z]{17}$"))
            {
                MessageBox.Show("Vin is invalid! Vin must contain 17 characters. ");
                return false;
            }
            if (!int.TryParse(tbYear.Text, out int year) || year < 1000 || year > 3000)
            {
                MessageBox.Show("Year must be a number, which is between 1000 - 3000. ");
                return false;
            }
            if (!Regex.IsMatch(tbMake.Text, "^[a-zA-Z0-9 .]{1,50}$"))
            {
                MessageBox.Show("Make must contain 1 - 50 characters. ");
                return false;
            }
            if (!Regex.IsMatch(tbModel.Text, "^[a-zA-Z0-9 .]{1,50}$"))
            {
                MessageBox.Show("Model must contain 1 - 50 characters. ");
                return false;
            }
            if (combobxDriveTrain.Text == "")
            {
                MessageBox.Show("DriveTrain cannot be empty. ");
                return false;
            }
            if (combobxStyle.Text == "")
            {
                MessageBox.Show("Style cannot be empty. ");
                return false;
            }
            if (combobxTransmission.Text == "")
            {
                MessageBox.Show("Transmission cannot be empty. ");
                return false;
            }
            return true;
        }

        private void lvVehicle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                selectedCar = lvVehicle.SelectedItem as Car;

                if (selectedCar == null) return;

                tbMake.Text = selectedCar.Make;
                tbModel.Text = selectedCar.Model;
                tbColor.Text = selectedCar.Color;
                tbYear.Text = selectedCar.Year.ToString();
                tbVin.Text = selectedCar.VIN;
                tbPrice.Text = selectedCar.Price.ToString();
                VehicleDlg_tbOdometer.Text = selectedCar.Odometer.ToString();
                tbVehicleId.Text = selectedCar.Id.ToString();

                if (selectedCar.Saleable == true) chkbxSaleable.IsChecked = true;

                if (selectedCar.New == true) rbNewCar.IsChecked = true;
                else rbUsedCar.IsChecked = true;

                combobxStyle.Text = Enum.GetName(selectedCar.Style.GetType(), selectedCar.Style);
                combobxDriveTrain.Text = Enum.GetName(selectedCar.Drive_Train.GetType(), selectedCar.Drive_Train);
                combobxTransmission.Text = Enum.GetName(selectedCar.Trans.GetType(), selectedCar.Trans);

                VehicleDlg_btnAddNewCar.Content = "Update";
                tbStatusBar.Text = "Vehicle Id: " + selectedCar.Id + " was selected. ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RefreshCars()
        {
            try
            {
                lvVehicle.ItemsSource = Globals.db.GetInventory();
                tbStatusBar.Text = "Inventory list has been refreshed. ";
            }
            catch (Exception ex)
            {
                if (ex is SqlException) MessageBox.Show(this, ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Clear()
        {
            try
            {
                tbMake.Text = "";
                tbModel.Text = "";
                tbColor.Text = "";
                tbYear.Text = "";
                tbVin.Text = "";
                tbPrice.Text = "";
                VehicleDlg_tbOdometer.Text = "";
                tbVehicleId.Text = "";
                chkbxSaleable.IsChecked = false;
                combobxStyle.Text = "";
                combobxDriveTrain.Text = "";
                combobxTransmission.Text = "";

                tbVehicleId.Text = Globals.db.GetNextCarId().ToString();
                VehicleDlg_btnAddNewCar.Content = "Add";
            }
            catch (Exception ex)
            {
                if (ex is SqlException) MessageBox.Show(this, ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void tbYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numbers.IsMatch(e.Text);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

    } //
} //
