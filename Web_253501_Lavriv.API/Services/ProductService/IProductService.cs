using Web_253501_Lavriv.Domain.Entities;
using Web_253501_Lavriv.Domain.Models;

namespace Web_253501_Lavriv.API.Services.ProductService
{
    public interface IProductService
    {
        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        /// <param name="categoryNormalizedName">Нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">Номер страницы списка</param>
        /// <param name="pageSize">Количество объектов на странице</param>
        /// <returns>Список объектов</returns>
        Task<ResponseData<ListModel<Detail>>> GetProductListAsync(
            string? categoryNormalizedName,
            int pageNo = 1,
            int pageSize = 3);

        /// <summary>
        /// Поиск объекта по Id
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>Найденный объект</returns>
        Task<ResponseData<Detail>> GetProductByIdAsync(int id);

        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="id">Id изменяемого объекта</param>
        /// <param name="product">Объект с новыми параметрами</param>
        Task UpdateProductAsync(int id, Detail product);

        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="id">Id удаляемого объекта</param>
        Task DeleteProductAsync(int id);

        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="product">Новый объект</param>
        /// <returns>Созданный объект</returns>
        Task<ResponseData<Detail>> CreateProductAsync(Detail product);

        /// <summary>
        /// Сохранить файл изображения для объекта
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="formFile">Файл изображения</param>
        /// <returns>Url к файлу изображения</returns>
        Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
    }
}
