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
    using WEB_253501_LAVRIV.Services.Authentication;
    using WEB_253501_LAVRIV.Services.FileService;

    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiProductService> _logger;
        private readonly IFileService _fileService; 
        private readonly ITokenAccessor _tokenAccessor;

        public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger, IFileService fileService, ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _fileService = fileService;
            _tokenAccessor = tokenAccessor; 
        }


        public async Task<ResponseData<ListModel<Detail>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

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

           
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

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
         
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var response = await _httpClient.GetAsync($"details/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Detail>>(_serializerOptions);
                return data;
            }

            _logger.LogError($"Error retrieving product by ID: {response.StatusCode}");
            return ResponseData<Detail>.Error($"Error: {response.StatusCode}");
        }



        public async Task UpdateProductAsync(int id, Detail product, IFormFile formFile)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var content = new MultipartFormDataContent
    {
        { new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json"), "product" }
    };

            if (formFile != null)
            {
                var stream = formFile.OpenReadStream();
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
                content.Add(fileContent, "file", formFile.FileName);
            }

            var response = await _httpClient.PutAsync($"api/products/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }


        public async Task DeleteProductAsync(int id)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.DeleteAsync($"details/{id}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Failed to delete product. Status: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Response Content: {responseContent}");
                throw new Exception($"Error: {response.StatusCode}, Reason: {response.ReasonPhrase}");
            }
        }

    }
}