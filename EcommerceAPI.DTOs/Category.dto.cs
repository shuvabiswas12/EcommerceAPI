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

    public class CategoryDTO
    {
        public string Id { get; set; } = null!;
        public string Name { set; get; } = null!;
    }

    public class CategoryUpdateDTO
    {
        public string Name { get; set; } = null!;
        public string Id { get; set; } = null!;
    }
}
