namespace WEB_253501_LAVRIV.Services.ProductService
{
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using NuGet.Protocol.Plugins;
    using Web_253501_Lavriv.Domain.Entities;
    using Web_253501_Lavriv.Domain.Models;

    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiProductService> _logger;

        public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public async Task<ResponseData<ListModel<Detail>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress}fruits/");
            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}/");
            }

            var response = await _httpClient.GetAsync(urlString.ToString());

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Detail>>>(_serializerOptions);
                return data;
            }

            _logger.LogError($"Error: {response.StatusCode}");
            return ResponseData<ListModel<Detail>>.Error($"Error: {response.StatusCode}");
        }

        Task<ResponseData<Detail>> IProductService.CreateProductAsync(Detail product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        Task IProductService.DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<ResponseData<Detail>> IProductService.GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task IProductService.UpdateProductAsync(int id, Detail product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }

}
