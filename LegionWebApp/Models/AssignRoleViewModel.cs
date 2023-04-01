using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LegionWebApp.Models
{
    public class AssignRoleViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}
