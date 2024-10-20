using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using Web_253501_Lavriv.API.Data;
using Web_253501_Lavriv.API.Services.ProductService;
using Web_253501_Lavriv.Domain.Entities;
using Web_253501_Lavriv.Domain.Models;

namespace Web_253501_Lavriv.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FruitsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private ProductService _productService;

        public FruitsController(AppDbContext context)
        {
            _context = context;
            _productService = new ProductService(context);
        }

        // GET: api/Details
        [HttpGet("{category?}")]
        public async Task<ActionResult<ResponseData<List<Detail>>>> GetDetails(
        string? category,
        int pageNo = 1,
        int pageSize = 3)
        {
            return Ok(await _productService.GetProductListAsync(
            category,
            pageNo,
            pageSize));
        }

        // POST: api/Details
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
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
    }
}
