using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SSMI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
            : base()
        {
            this.Groups = new HashSet<ApplicationUserGroup>();
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public int BrnchId { get; set; }

        public string UBrnchName { get; set; }

        public int DBrnchId { get; set; }

        public string DeptName { get; set; }

        public string NPassword { get; set; }
        public virtual ICollection<ApplicationUserGroup> Groups { get; set; }


    }

}