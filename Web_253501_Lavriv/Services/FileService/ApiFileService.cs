using System.Net.Http.Headers;
using WEB_253501_LAVRIV.Services.Authentication;

namespace WEB_253501_LAVRIV.Services.FileService
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenAccessor _tokenAccessor;

        public ApiFileService(HttpClient httpClient, ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _tokenAccessor = tokenAccessor; // Инициализация ITokenAccessor
        }


        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

            // Установить заголовок авторизации
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            // Создаем MultipartFormDataContent для загрузки файла
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
            content.Add(streamContent, "file", newName);

            // Отправляем запрос на сохранение файла
            var response = await _httpClient.PostAsync("", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return string.Empty;
        }


        public async Task DeleteFileAsync(string fileName)
        {
          
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var response = await _httpClient.DeleteAsync($"{fileName}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Ошибка при удалении файла: {response.StatusCode}");
            }
        }

    }
}
