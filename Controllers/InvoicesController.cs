using InvoiceSystem.Data;
using InvoiceSystem.Models;
using InvoiceSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InvoiceSystem.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public InvoicesController(ApplicationDbContext db) => _db = db;

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var invoices = await _db.InvoiceHeaders
                .Include(h => h.Customer)
                .ToListAsync();
            return View(invoices);
        }

        // GET: Invoices/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Customers = await _db.Customers.ToListAsync();
            ViewBag.Items = await _db.Items.ToListAsync();
            return View(new InvoiceCreateVM { OrderDate = DateOnly.FromDateTime(DateTime.Now) });
        }

        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] InvoiceCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _db.Customers.ToListAsync();
                ViewBag.Items = await _db.Items.ToListAsync();
                return View(vm);
            }

            Console.WriteLine($"Received Invoice: CustomerId={vm.CustomerId}, OrderDate={vm.OrderDate}");
            Console.WriteLine($"Raw LinesJson: {vm.LinesJson}");

            // Deserialize invoice lines from JSON
            var lines = JsonSerializer.Deserialize<List<InvoiceLineVM>>(vm.LinesJson ?? "[]");
            if (lines == null || lines.Count == 0)
            {
                ModelState.AddModelError("", "Invoice must have at least one line.");
                ViewBag.Customers = await _db.Customers.ToListAsync();
                ViewBag.Items = await _db.Items.ToListAsync();
                return View(vm);
            }

            var invoice = new InvoiceHeader
            {
                CustomerId = vm.CustomerId,
                OrderDate = vm.OrderDate
            };

            decimal total = 0m;
            foreach (var l in lines)
            {
                Console.WriteLine($"Line: {l}");
                var detail = new InvoiceDetail
                {
                    ItemId = l.ItemId,
                    Quantity = l.Quantity,
                    UnitPrice = l.UnitPrice
                };
                invoice.InvoiceDetails.Add(detail);
                total += l.UnitPrice * l.Quantity;
            }

            invoice.TotalFee = total;
            _db.InvoiceHeaders.Add(invoice);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", new { id = invoice.InvoiceHeaderId });
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var invoice = await _db.InvoiceHeaders
                .Include(h => h.InvoiceDetails)
                .FirstOrDefaultAsync(h => h.InvoiceHeaderId == id);

            if (invoice == null) return NotFound();

            ViewBag.Customers = await _db.Customers.ToListAsync();
            ViewBag.Items = await _db.Items.ToListAsync();

            // Prepare ViewModel
            var vm = new InvoiceEditVM
            {
                InvoiceHeaderId = invoice.InvoiceHeaderId,
                CustomerId = invoice.CustomerId,
                OrderDate = invoice.OrderDate,
                LinesJson = JsonSerializer.Serialize(invoice.InvoiceDetails.Select(d => new InvoiceLineVM
                {
                    ItemId = d.ItemId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList())
            };

            return View(vm);
        }

        // POST: Invoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] InvoiceEditVM vm)
        {
            if (id != vm.InvoiceHeaderId) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _db.Customers.ToListAsync();
                ViewBag.Items = await _db.Items.ToListAsync();
                return View(vm);
            }

            var invoice = await _db.InvoiceHeaders
                .Include(h => h.InvoiceDetails)
                .FirstOrDefaultAsync(h => h.InvoiceHeaderId == id);

            if (invoice == null) return NotFound();

            invoice.CustomerId = vm.CustomerId;
            invoice.OrderDate = vm.OrderDate;

            // Deserialize updated lines
            var lines = JsonSerializer.Deserialize<List<InvoiceLineVM>>(vm.LinesJson ?? "[]") ?? new List<InvoiceLineVM>();

            // Remove old details
            _db.InvoiceDetails.RemoveRange(invoice.InvoiceDetails);

            // Add new details
            decimal total = 0m;
            foreach (var l in lines)
            {
                var detail = new InvoiceDetail
                {
                    ItemId = l.ItemId,
                    Quantity = l.Quantity,
                    UnitPrice = l.UnitPrice
                };
                invoice.InvoiceDetails.Add(detail);
                total += l.UnitPrice * l.Quantity;
            }

            invoice.TotalFee = total;

            await _db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = invoice.InvoiceHeaderId });
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _db.InvoiceHeaders
                .Include(h => h.Customer)
                .Include(h => h.InvoiceDetails).ThenInclude(d => d.Item)
                .FirstOrDefaultAsync(h => h.InvoiceHeaderId == id);

            if (invoice == null) return NotFound();
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _db.InvoiceHeaders
                .Include(h => h.InvoiceDetails)
                .FirstOrDefaultAsync(h => h.InvoiceHeaderId == id);

            if (invoice == null) return NotFound();

            _db.InvoiceDetails.RemoveRange(invoice.InvoiceDetails);
            _db.InvoiceHeaders.Remove(invoice);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
