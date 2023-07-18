using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Model
{
    [Table("User")]
    public class User
    {
        [Key]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Dob { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string Gender { get; set; }
        public DateTime? LastLogin { get; set; }

    }
}
