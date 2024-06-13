using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;
        public string ProfilePhoto { get; set; } = null!;
    }
}
