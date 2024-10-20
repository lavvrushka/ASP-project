using Microsoft.EntityFrameworkCore;
using Web_253501_Lavriv.API.Data;
using Web_253501_Lavriv.Domain.Entities;
using Web_253501_Lavriv.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Web_253501_Lavriv.API.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly int _maxPageSize = 20;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<ListModel<Detail>>> GetProductListAsync(
      string? categoryNormalizedName,
      int pageNo = 1,
      int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.Details.AsQueryable();

            query = query.Where(d => categoryNormalizedName == null ||
                                     d.Category.NormalizedName.Equals(categoryNormalizedName));

            var count = await query.CountAsync();
            if (count == 0)
            {
                return ResponseData<ListModel<Detail>>.Success(new ListModel<Detail>());
            }

            var dataList = new ListModel<Detail>
            {
                TotalCount = count, // Общее количество объектов
                PageSize = pageSize,
                PageNumber = pageNo,
                Items = await query
                            .OrderBy(d => d.Id)
                            .Skip((pageNo - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync()
            };

            // Вычисление общего количества страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            // Вернем общий объект с данными
            return ResponseData<ListModel<Detail>>.Success(dataList);
        }

        public async Task<ResponseData<Detail>> GetProductByIdAsync(int id)
        {
            var detail = await _context.Details.FirstOrDefaultAsync(d => d.Id == id);
            if (detail == null)
            {
                return ResponseData<Detail>.Error("Продукт не найден");
            }
            return ResponseData<Detail>.Success(detail);
        }

        public async Task UpdateProductAsync(int id, Detail product)
        {
            var existingProduct = await _context.Details.FindAsync(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Продукт не найден");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Image = product.Image; // Обновите, если нужно

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var detail = await _context.Details.FindAsync(id);
            if (detail == null)
            {
                throw new KeyNotFoundException("Продукт не найден");
            }

            _context.Details.Remove(detail);
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseData<Detail>> CreateProductAsync(Detail product)
        {
            _context.Details.Add(product);
            await _context.SaveChangesAsync();
            return ResponseData<Detail>.Success(product);
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var detail = await _context.Details.FindAsync(id);
            if (detail == null)
            {
                return ResponseData<string>.Error("Продукт не найден");
            }

            if (formFile == null || formFile.Length == 0)
            {
                return ResponseData<string>.Error("Файл изображения не выбран");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, formFile.FileName);

            // Сохранение файла
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            detail.Image = filePath; // Обновите поле с изображением
            await _context.SaveChangesAsync();

            return ResponseData<string>.Success(filePath);
        }
    }
}
