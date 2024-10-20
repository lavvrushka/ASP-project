using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_253501_Lavriv.Domain.Entities
{
    public class Detail
    {
        // Уникальный идентификатор детали
        public int Id { get; set; }

        // Название детали
        public string Name { get; set; }

        // Описание детали
        public string? Description { get; set; }

        // Категория детали (навигационное свойство)
        public Category? Category { get; set; }

        // Цена детали
        public decimal Price { get; set; }

        // Путь к изображению детали
        public string? Image { get; set; } 

        // Mime-тип изображения (например, image/png, image/jpeg)
        public string? MimeType { get; set; }

        public int CategoryId { get; set; }
    }
}
