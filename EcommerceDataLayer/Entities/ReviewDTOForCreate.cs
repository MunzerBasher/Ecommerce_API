using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceDataLayer.DTOS
{
    public class ReviewDTOForCreate
    {
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public string ReviewText { get; set; }
        public decimal Rating { get; set; }
    }

}
