using InvoiceSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ItemsController(ApplicationDbContext db) => _db = db;

        // GET api/items/lookup?q=pen
        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Ok(new[] { new { id = 0, name = "", price = 0m } });

            var items = await _db.Items
                .Where(i => EF.Functions.Like(i.Name, $"%{q}%"))
                .OrderBy(i => i.Name)
                .Select(i => new { id = i.ItemId, name = i.Name, price = i.Price })
                .Take(10)
                .ToListAsync();

            return Ok(items);
        }
    }
}