using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_Project
{
    public class Orders
    {
        public long OrderId { get; set; }
        public long CustId { get; set; }
        public long EmployeeId { get; set; }
        public long CarId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PickupDate { get; set; }
        public bool Warranty { get; set; }
        public bool WinterTires { get; set; }
        public bool Rustproofing { get; set; }
        public double Total { get; set; }

        public Orders()
        {

        }

        public Orders(long id, long custId, long empId, long carId, DateTime orderDate, 
            DateTime pickupDate, bool warranty, bool winterTires, bool rustproofing, double total)
        {
            OrderId = id;
            CustId = custId;
            EmployeeId = empId;
            CarId = carId;
            OrderDate = orderDate;
            PickupDate = pickupDate;
            Warranty = warranty;
            WinterTires = winterTires;
            Rustproofing = rustproofing;
            Total = total;
        }

        public string ToCsvString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", OrderId, CustId, 
                EmployeeId, CarId, OrderDate, PickupDate, Warranty, WinterTires, Rustproofing, Total);
        }

        public string ToPdfString()
        {
            return string.Format("# : {0} - Sold: {1} - Warranty : {3} - WinterTires : {4} - Rustproofing : {5} - {2}", OrderId, 
                OrderDate, Warranty, WinterTires, Rustproofing, Total);
        }
    }
}
