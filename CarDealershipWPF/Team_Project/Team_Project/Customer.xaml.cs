using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer : Window
    {
        public Customer()
        {
            InitializeComponent();
            RefreshList();

        }

        private void RefreshList()
        {
            try
            {
                List<Cust> custList = Globals.db.GetAllCustomers();
                lvCustomer.ItemsSource = custList;

                // ListView filter
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvCustomer.ItemsSource);
                view.Filter = CustFilter;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(this, ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CustFilter(object item)
        {
            bool conditionName = (item as Cust).Name.IndexOf(tbContactName.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            bool conditionPhone = (item as Cust).PhoneNumber.IndexOf(tbPhone.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            bool conditionLicense = (item as Cust).DriversLicense.IndexOf(tbDriverLicense.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            bool conditionPostalCode = (item as Cust).PostalCode.IndexOf(tbPostalCode.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            bool conditionEmail = (item as Cust).Email.IndexOf(tbEmail.Text, StringComparison.OrdinalIgnoreCase) >= 0;

            return conditionName && conditionPhone && conditionLicense && conditionPostalCode && conditionEmail;
        }

        // for CustFilter
        private void tbContactName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvCustomer.ItemsSource).Refresh();
        }

        // for CustFilter
        private void tbPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvCustomer.ItemsSource).Refresh();
        }

        // for CustFilter
        private void tbDriverLicense_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvCustomer.ItemsSource).Refresh();
        }

        // for CustFilter
        private void tbPostalCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvCustomer.ItemsSource).Refresh();
        }

        // for CustFilter
        private void tbEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvCustomer.ItemsSource).Refresh();
        }

        private void ButtonNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddEditCustomerDialog dlg = new AddEditCustomerDialog(this);
            dlg.ShowDialog();
            RefreshList();
        }

        private void lvCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Cust currCust = lvCustomer.SelectedItem as Cust;
            if (currCust == null) return; 
            AddEditCustomerDialog dlg = new AddEditCustomerDialog(this, currCust);
            if (dlg.ShowDialog() == true)
            {
                RefreshList();
                MessageBox.Show("Change Saved!");
            }
        }

        private void CustomerDlg_btnUpdate_Click(object sender, RoutedEventArgs e) // Same function as lvCustomer_MouseDoubleClick
        {
            Cust currCust = lvCustomer.SelectedItem as Cust;
            if (currCust == null) return; 
            AddEditCustomerDialog dlg = new AddEditCustomerDialog(this, currCust);
            if (dlg.ShowDialog() == true)
            {
                RefreshList();
                MessageBox.Show("Change Saved!");
            }
            RefreshList();

        }

        private void CustomerDlg_btnReset_Click(object sender, RoutedEventArgs e)
        {
            tbContactName.Clear();
            tbPhone.Clear();
            tbDriverLicense.Clear();
            tbPostalCode.Clear();
            tbEmail.Clear();
        }

        // Return the selected customer to Transaction window
        private void CustomerDlg_btnSelect_Click(object sender, RoutedEventArgs e)
        {
            Cust currCust = lvCustomer.SelectedItem as Cust;
            if (currCust == null) return;
            DialogResult = true;
        }

        public Cust SelectedCustomer
        {
            get
            {
                Cust currCust = lvCustomer.SelectedItem as Cust;
                return currCust;
            }
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

                pdfTable3.AddCell(new Phrase(" Customer Information "));

                List<Cust> custList = Globals.db.GetAllCustomers();
                
                foreach (Cust c in custList)
                {
                    pdfTable2.AddCell(new Phrase(c.ToString()));
                }

                string folderPath = "C:\\PDF-Customers\\";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                int count = Directory.GetFiles("C:\\PDF-Customers").Length;
                string fileName = "Customers" + (count + 1) + ".pdf";

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
                List<Cust> allCustomers = Globals.db.GetAllCustomers();
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Id,Name,Address,City,Province,PostalCode,PhoneNumber,DriversLicense,Email");

                foreach (Cust c in allCustomers)
                {
                    csvContent.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                        c.Id, c.Name, c.Address, c.City, c.Province, c.PostalCode,c.PhoneNumber, c.DriversLicense, c.Email));
                }

                string folderPath = "C:\\CSV-Customers\\";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                int count = Directory.GetFiles("C:\\CSV-Customers").Length;
                string fileName = "CustomerList" + (count + 1) + ".csv";
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
    }

}
