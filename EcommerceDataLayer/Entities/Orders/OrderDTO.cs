using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceDataLayer.Entities.Orders
{
    public class OrderDTO
    {

        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
           public int Status { get; set; }
        public string  UserID { get; set; }
        public int  TotalAmount { get; set; }
    }
}
