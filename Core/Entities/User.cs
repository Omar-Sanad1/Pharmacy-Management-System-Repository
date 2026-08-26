using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RoleID { get; set; } // ==> FK
        public Role Role { get; set; } // ==> Navigation Property
        public Customer Customer { get; set; } // ==> Navigation Property
        public Employee Employee { get; set; } // ==> Navigation Property
    }
}
