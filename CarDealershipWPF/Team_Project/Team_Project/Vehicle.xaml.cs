using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for Vehicle.xaml
    /// </summary>
    

    public class Car
    {
        public long Id { get; set; }
        public string VIN { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public bool New { get; set; }
        public int Year { get; set; }
        public long Odometer { get; set; }
        public StyleEnum Style { get; set; }
        public TransmissionEnum Trans { get; set; }
        public DriveTrainEnum Drive_Train { get; set; }
        public string Color { get; set; }
        public double Price { get; set; }
        public bool Saleable { get; set; }

        public enum TransmissionEnum
        {
            Manual,
            Automatic,
            Tiptronic
        };

        public enum DriveTrainEnum
        {
            All_Wheel,
            Front_Wheel,
            Rear_Wheel
        };

        public enum StyleEnum
        {
            Sedan,
            Suv,
            Minivan,
            Pickup
        };

        public Car(long id, string vin, bool isNew, string make, string model, int year, StyleEnum style,
            TransmissionEnum trans, DriveTrainEnum dt, double price, long odometer, string color, bool saleable)
        {
            Id = id;
            VIN = vin;
            New = isNew;
            Make = make;
            Model = model;
            Year = year;
            Trans = trans;
            Drive_Train = dt;
            Price = price;
            Odometer = odometer;
            Color = color;
            Style = style;
            Saleable = saleable;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1} \t {2} \t {3} \nVIN: {4} \t New: {5} \nPrice: {6} \t {7} \t {8} \t {9} \n{10} km  \t {11}", 
                Id, Make, Model, Year, VIN, New, Price, Trans, Drive_Train, Style, Odometer, Color);
        }
    }

    public partial class Vehicle : Window
    {
        Regex numbers = new Regex("[^0-9]+");
        List<Car> activeVehicleList = Globals.db.GetInventory();
        public Car sc { get; set; }

        public object JOptionPane { get; private set; }

        public Vehicle(Window parent)
        {
            InitializeComponent();
            Owner = parent;
            RefreshCars();
            if (Main.currentEmployee == null) VehicleDlg_btnSelect.Visibility = Visibility.Hidden;
            
            // ListView filter
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvVehicle.ItemsSource);
            view.Filter = CarFilter;
        }

        private bool CarFilter(object item)
        {
            bool conditionMake = (item as Car).Make.IndexOf(tbMake.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            bool conditionModel = (item as Car).Model.IndexOf(tbModel.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            bool conditionYear = (item as Car).Year.ToString().IndexOf(tbYear.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            bool conditionStyle = (item as Car).Style.ToString().IndexOf(combobxStyle.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            bool conditionTrans = (item as Car).Trans.ToString().IndexOf(combobxTransmission.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            bool conditionDrivetrain = (item as Car).Drive_Train.ToString().IndexOf(combobxDriveTrain.Text, StringComparison.OrdinalIgnoreCase) >= 0;

            return conditionMake && conditionModel && conditionYear && conditionStyle && conditionTrans && conditionDrivetrain;
        }

        private void tbMake_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvVehicle.ItemsSource).Refresh();
            tbStatusBar.Text = "Search filtered by Make. ";
        }

        private void rbNewCar_Checked(object sender, RoutedEventArgs e)
        {
            gridVehicleCondition.IsEnabled = true;
            tbMake.Text = "Toyota";
            lblMake.IsEnabled = false;
            tbMake.IsEnabled = false;
            lblOdometer.IsEnabled = false;
            sldOdometer.IsEnabled = false;
            DateTime currentDate = DateTime.Now;
            int year = currentDate.Year;
            tbYear.Text = year.ToString();
            tbStatusBar.Text = "New cars. ";
        }

        private void rbUsedCar_Checked(object sender, RoutedEventArgs e)
        {
            gridVehicleCondition.IsEnabled = true;
            tbMake.Text = "";
            lblMake.IsEnabled = true;
            tbMake.IsEnabled = true;
            lblOdometer.IsEnabled = true;
            sldOdometer.IsEnabled = true;
            tbStatusBar.Text = "Used cars. ";
            tbYear.Text = "";
        }
        
        public void RefreshCars()
        {
            List<Car> allCars = Globals.db.GetInventory();
            List<Car> saleableCars = new List<Car>();

            foreach (Car c in allCars)
            {
                if (c.Saleable == true)
                {
                    saleableCars.Add(c);
                }
            }

            lvVehicle.ItemsSource = saleableCars;
            tbStatusBar.Text = "All saleable cars listed. ";
        }
    
        private void tbYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numbers.IsMatch(e.Text);
        }
        
        private void sldOdometer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VehicleDlg_tbOdometer.Text = sldOdometer.Value.ToString();
        }
 
        private void VehicleDlg_btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddUpdateCar dlg = new AddUpdateCar(this);
            dlg.ShowDialog();
            if (dlg.DialogResult == true) RefreshCars();
        }

        private void VehicleDlg_btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (lvVehicle.SelectedItem == null)
            {
                MessageBox.Show("Select a vehicle to continue. ");
                return;
            }
            sc = lvVehicle.SelectedItem as Car;
            DialogResult = true;
        }

        private void combobxStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvVehicle.ItemsSource).Refresh();
            tbStatusBar.Text = "Search complete. ";
        }

        private void VehicleDlg_btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbMake.Text = "";
            tbModel.Text = "";
            tbYear.Text = "";
            combobxDriveTrain.Text = "";
            combobxStyle.Text = "";
            combobxTransmission.Text = "";
            combobxPriceRange.Text = "";
            sldOdometer.Value = 0;
            lblOdometer.Content = "";
            tbStatusBar.Text = "Search filters have been cleared. ";
        }

        private void combobxPriceRange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            if (combobxPriceRange.SelectedIndex == 0) return;
            int min = 0;
            int max = 1000000;

            if (combobxPriceRange.SelectedIndex == 1) { max = 10000; }
            if (combobxPriceRange.SelectedIndex == 2) { min = 10000; max = 25000; }
            if (combobxPriceRange.SelectedIndex == 3) { min = 25000; max = 45000; }
            if (combobxPriceRange.SelectedIndex == 4) { min = 45000; max = 65000; }
            if (combobxPriceRange.SelectedIndex == 5) { min = 65000; }

            List<Car> allCars = Globals.db.GetInventory();
            List<Car> filteredByPrice = new List<Car>();

            foreach (Car c in allCars)
            {
                if (c.Saleable == true)
                {
                    if (c.Price >= min && c.Price <= max)
                    {
                        filteredByPrice.Add(c);
                    }
                }
            }

            lvVehicle.ItemsSource = filteredByPrice;
            tbStatusBar.Text = "All saleable cars listed by price range selected. ";
            */
        }

        private void btnExportToPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string image = @"..\..\toyota-logo.jpg";
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(image);

                jpg.ScaleToFit(120f, 100f);
                jpg.SpacingBefore = 5f;
                jpg.Alignment = Element.ALIGN_CENTER;

                PdfPTable pdfTableBlank = new PdfPTable(1);

                // Footer
                PdfPTable pdfTableFooter = new PdfPTable(1);
                pdfTableFooter.DefaultCell.BorderWidth = 0;
                pdfTableFooter.WidthPercentage = 100;
                pdfTableFooter.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                Chunk cnkFooter = new Chunk("Toyota 2018");
                cnkFooter.Font.Size = 10;
                pdfTableFooter.AddCell(new Phrase(cnkFooter));
                // end of Footer

                pdfTableBlank.AddCell(new Phrase(" "));
                pdfTableBlank.DefaultCell.BorderWidth = 0.0f;

                // parameter is number of columns
                PdfPTable pdfTable1 = new PdfPTable(1);
                PdfPTable pdfTable2 = new PdfPTable(2);
                PdfPTable pdfTable3 = new PdfPTable(1);

                pdfTable1.WidthPercentage = 80;
                pdfTable1.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable1.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                pdfTable1.DefaultCell.BorderWidth = 0;

                pdfTable2.DefaultCell.Padding = 10;
                pdfTable2.WidthPercentage = 100;
                pdfTable2.DefaultCell.BorderWidth = 0.0f;

                pdfTable3.WidthPercentage = 80;
                pdfTable3.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable3.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                pdfTable3.DefaultCell.BorderWidth = 0.5f;

                pdfTable1.AddCell(new Phrase(" 123 Street Name, City, Province, Postal Code "));
                pdfTable1.AddCell(new Phrase(" Customer Care : 123-4567-8901  |  321-654-9870 "));

                pdfTable3.AddCell(new Phrase("  Inventory List "));

                foreach (Car c in activeVehicleList)
                {
                    pdfTable2.AddCell(new Phrase(c.ToString()));
                }

                string folderPath = "C:\\PDF-Vehicles\\";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                int count = Directory.GetFiles("C:\\PDF-Vehicles").Length;
                string fileName = "Vehicles" + (count + 1) + ".pdf";

                using (FileStream stm = new FileStream(folderPath + fileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 10f, 0);
                    PdfWriter.GetInstance(pdfDoc, stm);
                    pdfDoc.Open();
                    pdfDoc.Add(jpg);
                    pdfDoc.Add(new iTextSharp.text.Paragraph(" "));
                    pdfDoc.Add(pdfTable1);
                    pdfDoc.Add(new iTextSharp.text.Paragraph(" "));
                    pdfDoc.Add(pdfTable3);
                    pdfDoc.Add(new iTextSharp.text.Paragraph(" "));
                    pdfDoc.Add(pdfTable2);
                    pdfDoc.Add(new iTextSharp.text.Paragraph(" "));
                    pdfDoc.Add(pdfTableFooter);
                    pdfDoc.Close();
                    stm.Close();

                    // displays pdf
                    // System.Diagnostics.Process.Start(folderPath + fileName);

                    MessageBox.Show("Document exported: " + folderPath + fileName);

                    tbStatusBar.Text = "pdf created and save to: " + folderPath + fileName;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnExportToCSV_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Car> allVehiclesList = Globals.db.GetInventory();

                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Id,Make,Model,New,Odometer,Price,Style," +
                    "Trans,VIN,Year,Drive_Train,Color");

                foreach (Car c in allVehiclesList)
                {
                    csvContent.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
                        c.Id, c.Make, c.Model, c.New, c.Odometer, c.Price, c.Style, 
                        c.Trans, c.VIN, c.Year, c.Drive_Train, c.Color));
                }

                string folderPath = "C:\\CSV-Vehicles\\";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                int count = Directory.GetFiles("C:\\CSV-Vehicles").Length;
                string fileName = "Vehicles" + (count + 1) + ".csv";
                string csvPath = folderPath + fileName;

                File.AppendAllText(csvPath, csvContent.ToString());

                MessageBox.Show("Document exported: " + csvPath);

                tbStatusBar.Text = "csv created and save to: " + folderPath + fileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    } //
} //
