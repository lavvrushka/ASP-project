using Microsoft.AspNetCore.Http;

namespace WEB_253501_LAVRIV.Extensions
{
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Проверяет, является ли запрос асинхронным (Ajax).
        /// </summary>
        /// <param name="request">HttpRequest объект.</param>
        /// <returns>True, если запрос является Ajax-запросом, иначе False.</returns>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
