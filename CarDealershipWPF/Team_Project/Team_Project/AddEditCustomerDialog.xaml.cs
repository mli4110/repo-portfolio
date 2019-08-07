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
    /// Interaction logic for AddEditCustomerDialog.xaml
    /// </summary>


    public class Cust
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string DriversLicense { get; set; }
        public string Email { get; set; }

        public Cust()
        {

        }

        public Cust(long id, string name, string address, string city, string province, 
            string postalcode, string phone, string driversLicense, string email)
        {
            Id = id;
            Name = name;
            Address = address;
            City = city;
            PostalCode = postalcode;
            Province = province;
            PhoneNumber = phone;
            DriversLicense = driversLicense;
            Email = email;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1} \t {2} \n{3}, {4}, {5}, {6} \n{7} \t {8}", Id, Name,
                PhoneNumber, Address, City, Province, PostalCode, DriversLicense, Email);
        }

    }


    public partial class AddEditCustomerDialog : Window
    {
        private Cust currCust;
        Regex postCode = new Regex("[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ][-? ?][0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]");

        public AddEditCustomerDialog(Window parent, Cust __currCust = null)
        {
            InitializeComponent();
            Owner = parent;

            currCust = __currCust;
            if (currCust != null)
            {
                tbContactName.Text = currCust.Name;
                tbDriverLicense.Text = currCust.DriversLicense;
                tbEmail.Text = currCust.Email;
                tbPhone.Text = currCust.PhoneNumber;
                tbPostalCode.Text = currCust.PostalCode;
                tbAddress.Text = currCust.Address;
                tbCity.Text = currCust.City;
                cbProvince.Text = currCust.Province;
            }
        }

        private void AddEditCustomerDialog_btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = tbContactName.Text.ToUpper();
                string address = tbAddress.Text.ToUpper();
                string city = tbCity.Text.ToUpper();
                string province = cbProvince.Text.ToUpper();
                string postalCode = tbPostalCode.Text.ToUpper();
                string phoneNumber = tbPhone.Text.ToUpper();
                string driversLicense = tbDriverLicense.Text.ToUpper();
                string email = tbEmail.Text.ToUpper();

                if (!Regex.IsMatch(name, "^[A-Z .-]{2,20}$"))
                {
                    MessageBox.Show(this, "Name must contain 2 - 20 letters.");
                    return;
                }
                if (!Regex.IsMatch(phoneNumber, "^[0-9]{10,13}$"))
                {
                    MessageBox.Show(this, "Phone Number is invalid, it must contain only 10 - 13 digits number.");
                    return;
                }
                if (!(Regex.IsMatch(email, "^[0-9A-Z -]{1,50}") && email.Contains('@') && email.Contains('.')))
                {
                    MessageBox.Show(this, "Email must contain . and @, and less than 50 characters.");
                    return;
                }
                if (!Regex.IsMatch(address, "^[0-9A-Z .]{1,50}$"))
                {
                    MessageBox.Show(this, "Address must contain 1 - 50 characters.");
                    return;
                }
                if (!Regex.IsMatch(driversLicense, "^[A-Z0-9]{13}$"))
                {
                    MessageBox.Show(this, "Driver's License number is invalid, it must contain 13 characters.");
                    return;
                }
                if (!Regex.IsMatch(city, "^[A-Z .-]{1,50}$"))
                {
                    MessageBox.Show(this, "City must contain 1 - 50 characters.");
                    return;
                }
                if (!postCode.IsMatch(postalCode))
                {
                    MessageBox.Show(this, "Postal code is unvalid.");
                    return;
                }

                if (currCust == null)
                {
                    Cust cust = new Cust()
                    {
                        Name = name,
                        Address = address,
                        City = city,
                        Province = province,
                        PostalCode = postalCode,
                        PhoneNumber = phoneNumber,
                        DriversLicense = driversLicense,
                        Email = email
                    };
                    Globals.db.AddCustomer(cust);
                }
                else
                {
                    currCust.Name = name;
                    currCust.Address = address;
                    currCust.City = city;
                    currCust.Province = province;
                    currCust.PostalCode = postalCode;
                    currCust.PhoneNumber = phoneNumber;
                    currCust.DriversLicense = driversLicense;
                    currCust.Email = email;

                    Globals.db.UpdateCustomer(currCust);
                }
                DialogResult = true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(this, ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
