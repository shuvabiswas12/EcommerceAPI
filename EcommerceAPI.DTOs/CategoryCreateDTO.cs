using EcommerceAPI.Utilities.Validation.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class CategoryCreateDTO
    {
        [NotEmpty("Category Name can not be empty.")] public string Name { get; set; } = null!;
    }
}
