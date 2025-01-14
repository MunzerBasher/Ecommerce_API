using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceDataLayer.DTOS
{
    public class ProductCategoryDTO
    {
        public int CategoryID { get; set; }
        public required string CategoryName { get; set; }
    }
}
