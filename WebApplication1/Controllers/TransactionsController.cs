using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.Entities;

namespace WebApplication1.Controllers;

public class TransactionsController : Controller
{
    private readonly ApplicationDbContext _context;

    public TransactionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Transactions
    public async Task<IActionResult> Index()
    {
        var transactions = await _context.Transactions
            .Include(t => t.Category)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
        return View(transactions);
    }

    // GET: Transactions/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.Accounts = await _context.Accounts.ToListAsync();
        return View();
    }

    // POST: Transactions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Date,Description,Amount,AccountID,CategoryId,Type,IsRecurring,RecurrenceInterval")] Transaction transaction)
    {
        if (ModelState.IsValid)
        {
            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.Accounts = await _context.Accounts.ToListAsync();
        return View(transaction);
    }

    // GET: Transactions/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return NotFound();
        }
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.Accounts = await _context.Accounts.ToListAsync();
        return View(transaction);
    }

    // POST: Transactions/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("TransactionID,Date,Description,Amount,AccountID,CategoryId,Type,IsRecurring,RecurrenceInterval")] Transaction transaction)
    {
        if (id != transaction.TransactionID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(transaction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(transaction.TransactionID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.Accounts = await _context.Accounts.ToListAsync();
        return View(transaction);
    }

    private bool TransactionExists(int id)
    {
        return _context.Transactions.Any(e => e.TransactionID == id);
    }
} 