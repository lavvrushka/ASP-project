namespace Web_253501_Lavriv.BlazorWasm.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Web_253501_Lavriv.Domain.Entities;

    public interface IDataService
    {
        // Событие, генерируемое при изменении данных
        event Action DataLoaded;

        // Список категорий
        List<Category> Categories { get; set; }

        // Список деталей
        List<Detail> Details { get; set; }

        // Признак успешного ответа на запрос
        bool Success { get; set; }

        // Сообщение об ошибке
        string ErrorMessage { get; set; }

        // Количество страниц
        int TotalPages { get; set; }

        // Номер текущей страницы
        int CurrentPage { get; set; }

        // Фильтр по категории
        Category SelectedCategory { get; set; }

        // Получение списка всех деталей
        Task GetDetailsListAsync(int pageNo = 1);

        // Получение списка категорий
        Task GetCategoryListAsync();

        // Метод для установки выбранной категории
        void SetSelectedCategory(Category category);
    }

}
