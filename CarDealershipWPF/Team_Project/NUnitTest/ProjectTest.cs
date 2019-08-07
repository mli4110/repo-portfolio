using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team_Project;

namespace NUnitTest
{
    [TestFixture]
    public class ProjectTest
    {
        Database db;

        [SetUp]
        public void Initialize()
        {
            db = new Database();
        }

        [TearDown]
        public void CleanUp()
        {
            db.Close();
        }

        [Test, Timeout(2000)]
        public void TestGetNextEmployeeId()
        {
            long nextId = db.GetNextEmployeeId();
            Employee emp1 = db.SelectEmployee(nextId);
            Employee emp2 = db.SelectEmployee(nextId - 1);
            Assert.IsNull(emp1);
            Assert.IsNotNull(emp2);
        }

        [Test, Timeout(2000)]
        public void TestAddEmployee()
        {
            Employee emp1 = new Employee()
            {
                Name = "testName1",
                Title = "Supervisor",
                PW = "welcome"
            };
            long currId = db.AddEmployee(emp1);
            long nextId = db.GetNextEmployeeId();
            Assert.AreEqual(nextId - currId, 1, "Next ID should + 1 on Current ID");
        }

        
        [Test, Timeout(2000)]
        public void TestGetAllOrdersAndGetCurrentMaxOrderId()
        {
            long maxOrderId = db.GetCurrentMaxOrderId();

            List<Orders> list = db.GetAllOrders();
            long lastOrderId = list[list.Count - 1].OrderId;

            Assert.AreEqual(maxOrderId, lastOrderId, "MaxOrderId shoule be same as lastOrderId");
        }
        

        [Test, Timeout(2000)]
        public void TestWarrantyPrice()
        {
            string str = "Warranties";
            double price = db.GetExtraOptionPrice(str);
            Assert.AreEqual(price, 500, "Warranty price should be 500");
        }

        [Test, Timeout(2000)]
        public void TestWinterTirePrice()
        {
            string str = "Winter Tires";
            double price = db.GetExtraOptionPrice(str);
            Assert.AreEqual(price, 600, "Winter tires price should be 600");
        }

        [Test, Timeout(2000)]
        public void TestRustProofingPrice()
        {
            string str = "Rust Proofing";
            double price = db.GetExtraOptionPrice(str);
            Assert.AreEqual(price, 200, "Rust proofing price should be 200");
        }

        [Test, Timeout(2000)]
        public void TestGetNextCustomerId()
        {
            List<Cust> custList = db.GetAllCustomers();
            long lastId = custList.Last().Id;
            long nextId = db.GetNextCustomerId();
            Assert.AreEqual(lastId + 1, nextId, "Next Id should be Last Id plus 1");
        }

        [Test, Timeout(2000)]
        public void TestGetNextCarId()
        {
            List<Car> carList = db.GetInventory();
            long lastId = carList.Last().Id;
            long nextId = db.GetNextCarId();
            Assert.AreEqual(lastId + 1, nextId, "Next Id should be Last Id plus 1");
        }
    }
}
