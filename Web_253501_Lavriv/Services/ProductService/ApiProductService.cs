namespace WEB_253501_LAVRIV.Services.ProductService
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using NuGet.Protocol.Plugins;
    using Web_253501_Lavriv.Domain.Entities;
    using Web_253501_Lavriv.Domain.Models;
    using WEB_253501_LAVRIV.Services.FileService;

    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiProductService> _logger;
        private readonly IFileService _fileService; // Поле для IFileService

        public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger, IFileService fileService)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _fileService = fileService; // Инициализация IFileService
        }

        public async Task<ResponseData<ListModel<Detail>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress}details/");
            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                urlString.Append($"{categoryNormalizedName}");
            }

            urlString.Append($"?pageNo={pageNo}");

            var response = await _httpClient.GetAsync(urlString.ToString());

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Detail>>>(_serializerOptions);
                return data;
            }

            _logger.LogError($"Error: {response.StatusCode}");
            return ResponseData<ListModel<Detail>>.Error($"Error: {response.StatusCode}");
        }

        public async Task<ResponseData<Detail>> CreateProductAsync(Detail product, IFormFile? formFile)
        {
           
            product.Image = "Images/noimage.jpg";

            
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);

               
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    product.Image = imageUrl;
                }
            }

         
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Details");

           
            var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
              
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Detail>>(_serializerOptions);
                return data;
            }

           
            _logger.LogError($"-----> object not created. Error: {response.StatusCode}");
            return ResponseData<Detail>.Error($"Объект не добавлен. Error: {response.StatusCode}");
        }


        public async Task<ResponseData<Detail>> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"details/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Detail>>(_serializerOptions);
                Console.WriteLine($"Retrieved Product: {data?.Data?.Name}"); // Проверка содержимого данных
                return data;
            }

            _logger.LogError($"Error retrieving product by ID: {response.StatusCode}");
            return ResponseData<Detail>.Error($"Error: {response.StatusCode}");
        }


        public async Task UpdateProductAsync(int id, Detail product, IFormFile? formFile)
        {
         
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    product.Image = imageUrl; 
                }
            }

            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"details/{id}");
            var response = await _httpClient.PutAsJsonAsync(uri, product, _serializerOptions);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error updating product: {response.StatusCode}");
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"details/{id}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error deleting product: {response.StatusCode}");
                throw new Exception($"Error: {response.StatusCode}");
            }
        }
    }
}