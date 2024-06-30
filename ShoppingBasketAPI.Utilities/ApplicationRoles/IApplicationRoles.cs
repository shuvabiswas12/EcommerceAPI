using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Utilities.ApplicationRoles
{
    public interface IApplicationRoles
    {
        private const string AdminRole = "Admin";
        private const string EmployeeRole = "Employee";
        private const string WebUserRole = "Web_User";

        public static string ADMIN { get; private set; } = AdminRole;
        public static string EMPLOYEE { get; private set; } = EmployeeRole;
        public static string WEB_USER { get; private set; } = WebUserRole;
    }
}
