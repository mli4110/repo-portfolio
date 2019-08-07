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
using System.IO;
using Microsoft.Win32;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.SqlClient;

namespace Team_Project
{
    /// <summary>
    /// Interaction logic for Stats.xaml
    /// </summary>
    public partial class Stats : Window
    {
        List<Orders> empList = new List<Orders>();
        Employee em = Main.currentEmployee;

        public Stats(Window parent)
        {
            InitializeComponent();
            Owner = parent;

            tbEmployeeID.Text = em.Id.ToString();
            tbEmployeeName.Text = em.Name.ToString();

            tbWeeklyGoal.Text = "$ 25,000";
            tbMonthlyGoal.Text = "$ 125,000";
            tbAnnualGoal.Text = "$ 6,500,000";

            GetEmpOrders();
            lvOrders.ItemsSource = empList;
            tbWeeklyActual.Text = GetWeekly();
            tbMonthlyActual.Text = GetMonthly();
            tbAnnualActual.Text = GetAnnual();
            tbStatusBar.Text = "Sales numbers updated. ";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        
        public DateTime StartOfWeekMonday()
        {
            DateTime start = DateTime.Now;
            try
            {
                DateTime dt = DateTime.Now;
                int diff = (7 + (DateTime.Now.DayOfWeek - DayOfWeek.Monday)) % 7;
                start = dt.AddDays(-1 * diff).Date;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return start;
        }
        

        public string GetWeekly()
        {
            double total = 0;

            try
            {
                DateTime StartOfWeek = StartOfWeekMonday();
                DateTime EndOfLastWeek = StartOfWeek.AddDays(+6);

                if (empList == null) return "$ 0";

                foreach (Orders o in empList)
                {
                    if (o.OrderDate > StartOfWeek && o.OrderDate < EndOfLastWeek)
                    {
                        total += o.Total;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return "$ " + total;
        }

        public string GetMonthly()
        {
            double total = 0;

            try
            {
                DateTime date = DateTime.Now;

                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                if (empList == null) return "$ 0";

                foreach (Orders o in empList)
                {
                    if (o.OrderDate > firstDayOfMonth && o.OrderDate < lastDayOfMonth)
                    {
                        total += o.Total;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return "$ " + total;
        }

        public string GetAnnual()
        {
            double total = 0;

            try
            {
                DateTime date = DateTime.Now;
                DateTime janFirst = new DateTime(date.Year, 1, 1);
                DateTime decLast = new DateTime(date.Year, 12, 31);

                if (empList == null) return "$ 0";

                foreach (Orders o in empList)
                {
                    if (o.OrderDate > janFirst && o.OrderDate < decLast)
                    {
                        total += o.Total;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return "$ " + total;
        }

        private void btnExportToPdf_Click(object sender, RoutedEventArgs e)
        {
            CreatePDF();
            tbStatusBar.Text = "pdf created and saved. ";
        }


        public void GetEmpOrders()
        {
            try
            {
                List<Orders> allOrders = Globals.db.GetAllOrders();
                empList.Clear();

                if (em.Title == "Manager")
                {
                    if (empList != null) empList.Clear();
                    empList = allOrders;
                    return;
                }

                foreach (Orders or in allOrders)
                {
                    if (or.EmployeeId == em.Id)
                    {
                        empList.Add(or);
                    }
                }
                tbStatusBar.Text = "Orders loaded. ";
            }
            catch (Exception ex)
            {
                if (ex is SqlException) MessageBox.Show(this, ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void CreatePDF()
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

                Chunk cnkFooter = new Chunk("Toyota");
                cnkFooter.Font.Size = 10;
                pdfTableFooter.AddCell(new Phrase(cnkFooter));
                // end of Footer

                pdfTableBlank.AddCell(new Phrase(" "));
                pdfTableBlank.DefaultCell.BorderWidth = 0.0f;

                // parameter is number of columns
                PdfPTable pdfTable1 = new PdfPTable(1);
                PdfPTable pdfTable2 = new PdfPTable(1);
                PdfPTable pdfTable3 = new PdfPTable(2);
                PdfPTable pdfTable4 = new PdfPTable(6);

                pdfTable1.WidthPercentage = 80;
                pdfTable1.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable1.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                pdfTable1.DefaultCell.BorderWidth = 0;

                pdfTable2.WidthPercentage = 80;
                pdfTable2.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable2.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                pdfTable2.DefaultCell.BorderWidth = 0;

                pdfTable3.DefaultCell.Padding = 5;
                pdfTable3.WidthPercentage = 80;
                pdfTable3.DefaultCell.BorderWidth = 0.5f;

                pdfTable4.DefaultCell.Padding = 5;
                pdfTable4.WidthPercentage = 95;
                pdfTable4.DefaultCell.BorderWidth = 0.0f;

                Chunk c1 = new Chunk("Toyota");
                c1.Font.Size = 15;

                Phrase p1 = new Phrase();
                p1.Add(c1);
                //    pdfTable1.AddCell(p1);

                Chunk c2 = new Chunk(" 123 Street Name, City, Province, Postal Code ");
                c2.Font.Size = 11;

                Phrase p2 = new Phrase();
                p2.Add(c2);
                pdfTable2.AddCell(p2);

                Chunk c3 = new Chunk(" Customer Care : 123-4567-8901  |  321-654-9870 ");
                c3.Font.Size = 11;

                Phrase p3 = new Phrase();
                p3.Add(c3);
                pdfTable2.AddCell(p3);

                // sales numbers
                pdfTable3.AddCell(new Phrase("Employee: " + em.Id + " " + em.Name));
                pdfTable3.AddCell(new Phrase("Current Date: " + DateTime.Now.ToShortDateString()));
                pdfTable3.AddCell(new Phrase());
                pdfTable3.AddCell(new Phrase());
                pdfTable3.AddCell(new Phrase("Weekly Sales-to-date: "));
                pdfTable3.AddCell(new Phrase(GetWeekly()));
                pdfTable3.AddCell(new Phrase("Monthly Sales-to-date: "));
                pdfTable3.AddCell(new Phrase(GetMonthly()));
                pdfTable3.AddCell(new Phrase("Annual Sales-to-date: "));
                pdfTable3.AddCell(new Phrase(GetAnnual()));

                // header for orders section
                pdfTable4.AddCell(new Phrase(" # "));
                pdfTable4.AddCell(new Phrase(" Date "));
                pdfTable4.AddCell(new Phrase(" Warranty "));
                pdfTable4.AddCell(new Phrase(" Winter Tires "));
                pdfTable4.AddCell(new Phrase(" RustProofing "));
                pdfTable4.AddCell(new Phrase(" Total "));
                
                foreach (Orders or in empList)
                {
                    string warranty = "No";
                    if (or.Warranty == true) warranty = "Yes";
                    string rp = "No";
                    if (or.Rustproofing == true) rp = "Yes";
                    string wt = "No";
                    if (or.WinterTires == true) wt = "Yes";

                    pdfTable4.AddCell(new Phrase(or.OrderId.ToString()));
                    pdfTable4.AddCell(new Phrase(or.OrderDate.ToShortDateString()));
                    pdfTable4.AddCell(new Phrase(warranty));
                    pdfTable4.AddCell(new Phrase(wt));
                    pdfTable4.AddCell(new Phrase(rp));
                    pdfTable4.AddCell(new Phrase("$ " + or.Total));
                }


                string folderPath = "C:\\PDF-" + em.Name + "\\";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                int count = Directory.GetFiles("C:\\PDF-" + em.Name).Length;
                string fileName = em.Id + "Sales" + (count + 1) + ".pdf";

                using (FileStream stm = new FileStream(folderPath + fileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 10f, 0);
                    PdfWriter.GetInstance(pdfDoc, stm);
                    pdfDoc.Open();
                    pdfDoc.Add(jpg);
                    pdfDoc.Add(new iTextSharp.text.Paragraph(" "));
                    pdfDoc.Add(pdfTable2);
                    pdfDoc.Add(new iTextSharp.text.Paragraph(" "));
                    pdfDoc.Add(pdfTable3);
                    pdfDoc.Add(new iTextSharp.text.Paragraph(" "));
                    pdfDoc.Add(pdfTable4);
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

        private void btnPrintToCSV_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder csvContent = new StringBuilder();

                csvContent.AppendLine("Employee ID,Employee Name");
                csvContent.AppendLine(string.Format("{0},{1}",em.Id,em.Name));

                csvContent.AppendLine(",");

                csvContent.AppendLine("Weekly Sales to date,Monthly Sales to date,Annual Sales to date");
                csvContent.AppendLine(string.Format("{0},{1},{2}",GetWeekly(),GetMonthly(),GetAnnual()));

                csvContent.AppendLine(",");

                csvContent.AppendLine("Order Id,Customer Id,Employee Id,Car Id,Order Date," +
                    "Pickup Date,Warranty,Winter Tires,RustProofing,Total");

                foreach (Orders or in empList)
                {
                    csvContent.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                        or.OrderId, or.CustId, or.EmployeeId, or.CarId, or.OrderDate, or.PickupDate,
                        or.Warranty, or.WinterTires, or.Rustproofing, or.Total));
                }

                string folderPath = "C:\\CSV-" + em.Name + "\\";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                int count = Directory.GetFiles("C:\\CSV-" + em.Name).Length;
                string fileName = em.Id + "Sales" + (count + 1) + ".csv";
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

    } // 
} // 

