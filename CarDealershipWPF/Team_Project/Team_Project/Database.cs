using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_Project
{
    public class Database
    {
        private const string CONN_STRING = "Server=tcp:ipd15-cardealership.database.windows.net,1433;Initial Catalog=CarDealerProject;" +
            "Persist Security Info=False;" +
            "User ID=sqladmin;Password=projectIPD15;" +
            "MultipleActiveResultSets=False;" +
            "Encrypt=True;TrustServerCertificate=False;" +
            "Connection Timeout=30;";

        private SqlConnection conn = new SqlConnection(CONN_STRING);

        public Database()
        {
            conn.Open();
        }

        public void Close()
        {
            conn.Close();
            conn = null;
        }

        public List<Cust> GetAllCustomers()
        {
            List<Cust> customerList = new List<Cust>();

            using (SqlCommand cm = new SqlCommand("SELECT * FROM Customers", conn))
            {
                using (SqlDataReader rd = cm.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        long id = (int)rd["Id"]; // (long) unable to convert
                        string name = (string)rd["Name"];
                        string address = (string)rd["Address"];
                        string city = (string)rd["City"];
                        string province = (string)rd["Province"];
                        string postalCode = (string)rd["PostalCode"];
                        string phone = (string)rd["PhoneNumber"];
                        string driversLicense = (string)rd["DriversLicense"];
                        string email = (string)rd["Email"];

                        Cust c = new Cust()
                        {
                            Id = id,
                            Name = name,
                            Address = address,
                            City = city,
                            Province = province,
                            PhoneNumber = phone,
                            DriversLicense = driversLicense,
                            Email = email,
                            PostalCode = postalCode
                        };

                        customerList.Add(c);
                    }
                }
            }
            return customerList;
        }


        public void AddCustomer(Cust c)
        {
            using (SqlCommand cm = new SqlCommand(
                "INSERT INTO Customers (Name, Address, City, Province, PostalCode, PhoneNumber, DriversLicense, Email) " +
                "VALUES (@ContactName, @Address, @City, @Province, @PostalCode, @PhoneNumber, @DriversLicense, @Email)", conn))
            {
                cm.Parameters.AddWithValue("@ContactName", c.Name);
                cm.Parameters.AddWithValue("@Address", c.Address);
                cm.Parameters.AddWithValue("@City", c.City);
                cm.Parameters.AddWithValue("@Province", c.Province);
                cm.Parameters.AddWithValue("@PostalCode", c.PostalCode);
                cm.Parameters.AddWithValue("@PhoneNumber", c.PhoneNumber);
                cm.Parameters.AddWithValue("@DriversLicense", c.DriversLicense);
                cm.Parameters.AddWithValue("@Email", c.Email);
                cm.ExecuteNonQuery();
            }
        }


        public void UpdateCustomer(Cust c)
        {
            using (SqlCommand update = new SqlCommand(
                "UPDATE Customers SET Name = @Name, Address = @Address, City = @City, Province = @Province, " +
                "PostalCode = @PostalCode, PhoneNumber = @Phone, DriversLicense = @DriversLicense, Email = @Email " +
                "WHERE Id = @Id", conn))
            {
                update.Parameters.AddWithValue("@Id", c.Id);
                update.Parameters.AddWithValue("@Name", c.Name);
                update.Parameters.AddWithValue("@Address", c.Address);
                update.Parameters.AddWithValue("@City", c.City);
                update.Parameters.AddWithValue("@Province", c.Province);
                update.Parameters.AddWithValue("@PostalCode", c.PostalCode);
                update.Parameters.AddWithValue("@Phone", c.PhoneNumber);
                update.Parameters.AddWithValue("@DriversLicense", c.DriversLicense);
                update.Parameters.AddWithValue("@Email", c.Email);
                update.ExecuteNonQuery();
            }
        }


        public long GetNextCustomerId()
        {
            using (SqlCommand get = new SqlCommand("SELECT top 1 * FROM Customers order by Id desc", conn))
            {
                long id = 1;
                using (SqlDataReader rd = get.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        Console.WriteLine(rd["Id"]);
                        id = long.Parse(rd["Id"].ToString());
                        Console.WriteLine(id);
                        return id + 1;
                    }
                    return id;
                }
            }
        }


        public List<Car> GetInventory()
        {
            List<Car> carInventory = new List<Car>();

            using (SqlCommand cm = new SqlCommand("SELECT * FROM Car", conn))
            {
                using (SqlDataReader rd = cm.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        long id = (int)rd["Id"];
                        string vin = rd["Vin"].ToString();
                        string make = rd["Make"].ToString();
                        string model = rd["Model"].ToString();
                        int year = (int)rd["Year"];
                        double price = (double)rd["Price"];
                        long odometer = (int)rd["Odometer"];
                        string color = rd["Color"].ToString();

                        bool isSaleable = false;
                        string strSaleable = rd["Saleable"].ToString();
                        if (strSaleable == "Yes") isSaleable = true;

                        bool isNew = false;
                        string strNew = rd["New"].ToString();
                        if (strNew == "New") isNew = true;

                        string transportStr = rd["Transmission"].ToString();
                        Car.TransmissionEnum trans;
                        bool isTrans = Enum.TryParse(transportStr, out trans);

                        string driveTrainstr = rd["Drivetrain"].ToString();
                        Car.DriveTrainEnum driveTrain;
                        bool isDriveTrain = Enum.TryParse(driveTrainstr, out driveTrain);

                        string styleStr = rd["Style"].ToString();
                        Car.StyleEnum style;
                        bool isStyle = Enum.TryParse(styleStr, out style);

                        Car c = new Car(id, vin, isNew, make, model, year, style,
                            trans, driveTrain, price, odometer, color, isSaleable);

                        carInventory.Add(c);
                    }
                }
            }
            return carInventory;
        }


        public void AddCar(Car c)
        {
            using (SqlCommand cm = new SqlCommand(
                "INSERT INTO Car (Vin, Make, Model, Year, Style, Transmission, Drivetrain, Color, Odometer, Price, " +
                "New, Saleable) VALUES (@Vin, @Make, @Model, @Year, @Style, @Transmission, @Drivetrain, @Color, " +
                "@Odometer, @Price, @New, @Saleable)", conn))
            {
                cm.Parameters.AddWithValue("@Vin", c.VIN);
                cm.Parameters.AddWithValue("@Make", c.Make);
                cm.Parameters.AddWithValue("@Model", c.Model);
                cm.Parameters.AddWithValue("@Year", c.Year);
                cm.Parameters.AddWithValue("@Style", c.Style.ToString());
                cm.Parameters.AddWithValue("@Transmission", c.Trans.ToString());
                cm.Parameters.AddWithValue("@Drivetrain", c.Drive_Train.ToString());
                cm.Parameters.AddWithValue("@Color", c.Color);
                cm.Parameters.AddWithValue("@Odometer", c.Odometer);
                cm.Parameters.AddWithValue("@Price", c.Price);
                string isNew = "Used";
                if (c.New == true) isNew = "New";
                cm.Parameters.AddWithValue("@New", isNew);
                string saleable = "No";
                if (c.Saleable == true) saleable = "Yes";
                cm.Parameters.AddWithValue("@Saleable", saleable);
                cm.ExecuteNonQuery();
            }
        }


        public void DeleteCar(Car c)
        {
            using (SqlCommand cm = new SqlCommand(
                "DELETE FROM Car WHERE Id = @Id", conn))
            {
                cm.Parameters.AddWithValue("@Id", c.Id);
                cm.ExecuteNonQuery();
            }
        }


        public void UpdateCar(Car c)
        {
            using (SqlCommand cm = new SqlCommand(
                "UPDATE Car SET Vin = @Vin, Make = @Make, Model = @Model, Year = @Year, Style = @Style, " +
                "Transmission = @Transmission, Drivetrain = @Drivetrain, Color = @Color, Odometer = @Odometer, " +
                "Price = @Price, New = @New, Saleable = @Saleable WHERE Id = @Id", conn))
            {
                cm.Parameters.AddWithValue("@Id", c.Id);
                cm.Parameters.AddWithValue("@Vin", c.VIN);
                cm.Parameters.AddWithValue("@Make", c.Make);
                cm.Parameters.AddWithValue("@Model", c.Model);
                cm.Parameters.AddWithValue("@Year", c.Year);
                cm.Parameters.AddWithValue("@Style", c.Style.ToString());
                cm.Parameters.AddWithValue("@Transmission", c.Trans.ToString());
                cm.Parameters.AddWithValue("@Drivetrain", c.Drive_Train.ToString());
                cm.Parameters.AddWithValue("@Color", c.Color);
                cm.Parameters.AddWithValue("@Odometer", c.Odometer);
                cm.Parameters.AddWithValue("@Price", c.Price);
                string isNew = "Used";
                if (c.New == true) isNew = "New";
                cm.Parameters.AddWithValue("@New", isNew);
                string saleable = "No";
                if (c.Saleable == true) saleable = "Yes";
                cm.Parameters.AddWithValue("@Saleable", saleable);
                cm.ExecuteNonQuery();
            }
        }


        public long GetNextCarId()
        {
            using (SqlCommand get = new SqlCommand("SELECT top 1 * FROM Car order by Id desc", conn))
            {
                long id = 1;
                using (SqlDataReader rd = get.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        Console.WriteLine(rd["Id"]);
                        id = long.Parse(rd["Id"].ToString());
                        Console.WriteLine(id);
                        return id + 1;
                    }
                    return id;
                }
            }
        }


        public List<Employee> GetEmployeeList()
        {
            List<Employee> employeeList = new List<Employee>();

            using (SqlCommand cm = new SqlCommand("SELECT * FROM Employees", conn))
            {
                using (SqlDataReader rd = cm.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        long id = (int)rd["Id"];
                        string name = rd["Name"].ToString();
                        string title = rd["Title"].ToString();
                        string pw = rd["Password"].ToString();

                        Employee em = new Employee(id, name, title, pw);
                        employeeList.Add(em);
                    }
                }
            }
            return employeeList;
        }


        public long AddEmployee(Employee em)
        {
            using (SqlCommand cm = new SqlCommand(
                "INSERT INTO Employees (Name, Title, Password) OUTPUT Inserted.ID VALUES (@Name, @Title, @Password)", conn))
            {
                cm.Parameters.AddWithValue("@Name", em.Name);
                cm.Parameters.AddWithValue("@Title", em.Title);
                cm.Parameters.AddWithValue("@Password", em.PW);
                long id = (int)cm.ExecuteScalar();
                em.Id = id;
                return id;
            }
        }


        public void UpdateEmployee(Employee em)
        {
            using (SqlCommand cm = new SqlCommand(
                "UPDATE Employees SET Name = @Name, Title = @Title, Password = @Password WHERE Id = @Id", conn))
            {
                cm.Parameters.AddWithValue("@Id", em.Id);
                cm.Parameters.AddWithValue("@Name", em.Name);
                cm.Parameters.AddWithValue("@Title", em.Title);
                cm.Parameters.AddWithValue("@Password", em.PW);
                cm.ExecuteNonQuery();
            }
        }

         
        public Employee SelectEmployee(long id)
        {
            Employee em = null;
            using (SqlCommand get = new SqlCommand("SELECT * FROM Employees WHERE Id = @Id", conn))
            {
                get.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader rd = get.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        long emId = (int)rd["Id"];
                        string name = rd["Name"].ToString();
                        string title = rd["Title"].ToString();
                        string pw = rd["Password"].ToString();

                        em = new Employee(emId, name, title, pw);
                    }
                }
            }
            return em;
        }
        

        public long GetNextEmployeeId()
        {
            using (SqlCommand get = new SqlCommand("SELECT top 1 * FROM Employees order by Id desc", conn))
            {
                long id = 1;
                using (SqlDataReader rd = get.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        Console.WriteLine(rd["Id"]);
                        id = long.Parse(rd["Id"].ToString());
                        Console.WriteLine(id);
                        return id + 1;
                    }
                    return id;
                }
            }
        }


        public List<Orders> GetAllOrders()
        {
            List<Orders> orderList = new List<Orders>();

            using (SqlCommand cm = new SqlCommand("SELECT * FROM Orders", conn))
            {
                using (SqlDataReader rd = cm.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        long id = (int)rd["Id"];
                        long custId = (int)rd["CustomerId"];
                        long empId = (int)rd["EmployeeId"];
                        long carId = (int)rd["CarId"];

                        bool isOrderDate = DateTime.TryParse(rd["OrderDate"].ToString(), out DateTime orderDate);
                        bool isPickupDate = DateTime.TryParse(rd["PickupDate"].ToString(), out DateTime pickupDate);

                        bool warranty = false;
                        if (rd["Warranty"].ToString() == "1") warranty = true;

                        bool winterTires = false;
                        if (rd["WinterTires"].ToString() == "1") winterTires = true;

                        bool rustproofing = false;
                        if (rd["RustProofing"].ToString() == "1") rustproofing = true;

                        string strTotal = rd["Total"].ToString();
                        double total = double.Parse(strTotal);

                        Orders o = new Orders(id, custId, empId, carId, orderDate, pickupDate, warranty, winterTires, rustproofing, total);
                        orderList.Add(o);
                    }
                }
            }
            return orderList;
        }


        public void AddOrder(Orders o)
        {
            using (SqlCommand cm = new SqlCommand(
                "INSERT INTO Orders (CustomerId, EmployeeId, CarId, OrderDate, PickupDate, Warranty, WinterTires, RustProofing, Total) " +
                "VALUES (@CustomerId, @EmployeeId, @CarId, @OrderDate, @PickupDate, @Warranty, @WinterTires, @RustProofing, @Total)", conn))
            {
                cm.Parameters.AddWithValue("@CustomerId", o.CustId);
                cm.Parameters.AddWithValue("@EmployeeId", o.EmployeeId);
                cm.Parameters.AddWithValue("@CarId", o.CarId);
                cm.Parameters.AddWithValue("@OrderDate", o.OrderDate);
                cm.Parameters.AddWithValue("@PickupDate", o.PickupDate);
                cm.Parameters.AddWithValue("@Warranty", o.Warranty);
                cm.Parameters.AddWithValue("@WinterTires", o.WinterTires);
                cm.Parameters.AddWithValue("@RustProofing", o.Rustproofing);
                cm.Parameters.AddWithValue("@Total", o.Total);
                cm.ExecuteNonQuery();
            }
        }

        public long GetCurrentMaxOrderId()
        {
            using (SqlCommand get = new SqlCommand("SELECT top 1 * FROM Orders order by Id desc", conn))
            {
                long id = 0;
                using (SqlDataReader rd = get.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        Console.WriteLine(rd["Id"]);
                        id = long.Parse(rd["Id"].ToString());
                    }
                    return id;
                }
            }
        }

        public double GetExtraOptionPrice(string name)
        {
            double price = 0;
            using (SqlCommand command = new SqlCommand("SELECT * FROM OptionRef WHERE OptionName = @Name", conn))
            {
                command.Parameters.AddWithValue("@Name", name);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        price = (double)reader["Price"];
                    }
                }
            }
            return price;
        }


    } // MW
} // NS

