using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_253501_Lavriv.API.Data;
using Web_253501_Lavriv.API.Services.ProductService;
using Web_253501_Lavriv.Domain.Entities;
using Web_253501_Lavriv.Domain.Models;

namespace Web_253501_Lavriv.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ProductService _productService;

        public DetailsController(AppDbContext context)
        {
            _context = context;
            _productService = new ProductService(context);
        }

        // GET: api/details or api/details/category
        [HttpGet("{category?}")]
        [Authorize] // Разрешить доступ всем пользователям
        public async Task<ActionResult<ResponseData<List<Detail>>>> GetDetails(
            string? category, int pageNo = 1, int pageSize = 3)
        {
            if (pageNo <= 0) pageNo = 1;
            if (pageSize <= 0) pageSize = 3;

            var result = await _productService.GetProductListAsync(category, pageNo, pageSize);
            return Ok(result);
        }

        // GET: api/details/5
        [HttpGet("{id:int}")]
        [AllowAnonymous] // Разрешить доступ всем пользователям
        public async Task<ActionResult<ResponseData<Detail>>> GetDetail(int id)
        {
            var detail = await _context.Details.FindAsync(id);
            if (detail == null)
            {
                return NotFound();
            }

            return Ok(new ResponseData<Detail> { Data = detail });
        }

        // POST: api/details
        [HttpPost]
        [Authorize(Policy = "admin")] // Доступ только для пользователей с политикой "admin"
        public async Task<ActionResult<Detail>> PostDetail(Detail detail)
        {
            _context.Details.Add(detail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetail", new { id = detail.Id }, detail);
        }

        private bool DetailExists(int id)
        {
            return _context.Details.Any(e => e.Id == id);
        }

        // PUT: api/details/5
        [HttpPut("{id:int}")]
        
        public async Task<IActionResult> PutDetail(int id, Detail detail)
        {
            if (id != detail.Id)
            {
                return BadRequest();
            }

            _context.Entry(detail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Успешное обновление
        }

        // DELETE: api/details/5
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "admin")] // Доступ только для пользователей с политикой "admin"
        public async Task<IActionResult> DeleteDetail(int id)
        {
            var detail = await _context.Details.FindAsync(id);
            if (detail == null)
            {
                return NotFound(); // Вернуть 404, если сущность не найдена
            }

            _context.Details.Remove(detail);
            await _context.SaveChangesAsync();

            return NoContent(); // Успешное удаление
        }
    }
}
