using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceDataLayer.Entities.Users
{
    public class UserDTO
    {
        public required string UserName { get; set; }
        public required string UserEmail { get; set; }
        public required string UserPhone { get; set; }
        public int UserVerflyCode { get; set; }
        public int UserApprove { get; set; }
        public required string UserDate { get; set; }
        public int UserPermission { get; set; }
        public required string UserPassword { get; set; }
        public int UserId { get; set; }
    }

}
