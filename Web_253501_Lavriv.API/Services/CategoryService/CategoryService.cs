﻿using Microsoft.EntityFrameworkCore;
using Web_253501_Lavriv.API.Data;
using Web_253501_Lavriv.Domain.Entities;
using Web_253501_Lavriv.Domain.Models;

namespace Web_253501_Lavriv.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return ResponseData<List<Category>>.Success(categories);
        }

    }
}
