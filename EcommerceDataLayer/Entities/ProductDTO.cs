using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceDataLayer.DTOS
{
    public class ProductDTO
    {
        public int ProductID { get; set; }
        public required string ProductName { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; } // Use decimal instead of SMALLMONEY
        public int QuantityInStock { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get;  set; }
    }

}
