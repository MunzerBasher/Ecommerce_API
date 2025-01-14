using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceDataLayer.DTOS
{
    public class Review
    {
        public int ReviewId { get; set; }
        public required string ReviewText { get; set; }
        public required string ProductName { get; set; }
        public required string UserName { get; set; }
        public required int Rating { get; set; }
        public required DateTime ReviewDate { get; set; }
    }
}
