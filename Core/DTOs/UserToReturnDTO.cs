using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class UserToReturnDTO
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RoleID { get; set; } 
    }
}
