using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Domain
{
    public class Category
    {
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required] public string Name { get; set; } = null!;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
