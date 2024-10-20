using System;

namespace Web_253501_Lavriv.Domain.Models
{
    public class ResponseData<T>
    {
        // Запрашиваемые данные
        public T? Data { get; set; }

        // Признак успешного завершения запроса
        public bool Successfull { get; set; } = true;

        // Сообщение в случае неуспешного завершения
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Получить объект успешного ответа
        /// </summary>
        /// <param name="data">Передаваемые данные</param>
        /// <returns></returns>
        public static ResponseData<T> Success(T data)
        {
            return new ResponseData<T>
            {
                Data = data,
                Successfull = true, // Убедитесь, что Successfull установлен в true
                ErrorMessage = null // Сообщение об ошибке должно быть null
            };
        }

        /// <summary>
        /// Получение объекта ответа с ошибкой
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        /// <param name="data">Передаваемые данные</param>
        /// <returns></returns>
        public static ResponseData<T> Error(string message, T? data = default)
        {
            return new ResponseData<T>
            {
                ErrorMessage = message,
                Successfull = false, // Убедитесь, что Successfull установлен в false
                Data = data
            };
        }
    }
}
