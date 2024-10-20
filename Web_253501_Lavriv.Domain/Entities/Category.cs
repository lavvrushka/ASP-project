using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_253501_Lavriv.Domain.Entities
{
    public class Category
    {
        // Уникальный идентификатор категории
        public int Id { get; set; }

        // Название категории
        public string Name { get; set; }

        // Название в формате "kebab-case" для маршрутов
        public string NormalizedName { get; set; }
    }
}
