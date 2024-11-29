using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_253501_Lavriv.Domain.Models
{
    public class ListModel<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        // Добавляем вычисляемые свойства
        public int CurrentPage => PageNumber;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
   
}
