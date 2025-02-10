using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceDataLayer.Entities.Users
{
    public class VerifyCodeRequest
    {
        public required string Email { get; set; }
        public required int VerifyCode { get; set; }
    }

}
