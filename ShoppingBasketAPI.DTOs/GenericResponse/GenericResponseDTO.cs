using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.DTOs.GenericResponse
{
    public class GenericResponseDTO<T> where T : class
    {
        public required int Count { get; set; }
        public required IEnumerable<T> Data { get; set; }
    }
}
