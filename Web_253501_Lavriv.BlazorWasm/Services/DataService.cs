using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Web_253501_Lavriv.Domain.Entities;
using Web_253501_Lavriv.Domain.Models;

namespace Web_253501_Lavriv.BlazorWasm.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly string _pageSize;
        private readonly IAccessTokenProvider _tokenProvider;

        public event Action DataLoaded;

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Detail> Details { get; set; } = new List<Detail>();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public Category SelectedCategory { get; set; }

        public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiBaseUrl"];
            _pageSize = configuration["PageSize"];
            _tokenProvider = tokenProvider;
        }

        // Метод для получения списка категорий
        public async Task GetCategoryListAsync()
        {
            try
            {
                // Получаем токен из IAccessTokenProvider
                var tokenRequest = await _tokenProvider.RequestAccessToken();
                if (tokenRequest.TryGetToken(out var token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);
                }

                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/categories");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response: {responseContent}"); // Логируем ответ

                    // Десериализуем ответ с учетом поля "data"
                    var categoriesResponse = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>();

                    if (categoriesResponse?.Successfull == true)
                    {
                        Categories = categoriesResponse.Data; // Присваиваем список категорий
                    }
                    else
                    {
                        ErrorMessage = categoriesResponse?.ErrorMessage ?? "No categories found.";
                    }
                }
                else
                {
                    ErrorMessage = "Failed to load categories.";
                }

                DataLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Exception occurred: {ex.Message}";
            }
        }


        public async Task GetDetailsListAsync(int pageNo = 1)
        {
            try
            {
                string route = "details/";

                // Проверяем, есть ли категория
                if (SelectedCategory != null)
                {
                    route += $"{SelectedCategory.NormalizedName}/";
                }

                var queryData = new List<KeyValuePair<string, string>>();

                if (pageNo > 1)
                {
                    queryData.Add(KeyValuePair.Create("pageNo", pageNo.ToString()));
                }

                if (!_pageSize.Equals("3"))
                {
                    queryData.Add(KeyValuePair.Create("pageSize", _pageSize));
                }

                var queryString = new StringBuilder();
                if (queryData.Count > 0)
                {
                    queryString.Append("?");
                    foreach (var pair in queryData)
                    {
                        queryString.Append($"{pair.Key}={pair.Value}&");
                    }
                    queryString.Remove(queryString.Length - 1, 1); // Удаляем последний символ '&'
                }

                var fullUrl = $"{_apiBaseUrl}/{route}{queryString}";

                var tokenRequest = await _tokenProvider.RequestAccessToken();
                if (tokenRequest.TryGetToken(out var token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);
                }

                var response = await _httpClient.GetAsync(fullUrl);
                if (response.IsSuccessStatusCode)
                {
                    var detailsResponse = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Detail>>>();
                    if (detailsResponse?.Successfull == true)
                    {
                        Details = detailsResponse.Data.Items;
                        TotalPages = detailsResponse.Data.TotalPages;  // Проверка, что данные пришли
                        CurrentPage = pageNo;

                        // Выводим информацию в консоль
                        Console.WriteLine($"Loaded {Details.Count} details. Total Pages: {TotalPages}, Current Page: {CurrentPage}");
                    }
                    else
                    {
                        ErrorMessage = detailsResponse?.ErrorMessage ?? "Error loading details";
                    }
                }
                else
                {
                    ErrorMessage = "Failed to load details";
                }

                DataLoaded?.Invoke();  // Сигнализируем о завершении загрузки данных
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Exception occurred: {ex.Message}";
            }
        }



        // Метод для установки выбранной категории и обновления данных
        public void SetSelectedCategory(Category category)
        {
            SelectedCategory = category;
            GetDetailsListAsync(1);  // Обновляем список объектов после выбора категории
        }

       

    }
}
