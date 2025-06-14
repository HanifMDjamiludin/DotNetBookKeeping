using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
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
            .Include(t => t.Account)
            .Include(t => t.Category)
            .ToListAsync();
        return View(transactions);
    }

    // GET: Transactions/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transaction = await _context.Transactions
            .Include(t => t.Account)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.TransactionID == id);

        if (transaction == null)
        {
            return NotFound();
        }

        return View(transaction);
    }

    // GET: Transactions/Create
    public async Task<IActionResult> Create()
    {
        try
        {
            var accounts = await _context.Accounts.ToListAsync();
            var categories = await _context.Categories.ToListAsync();

            if (!accounts.Any())
            {
                TempData["Error"] = "Please create an account first before creating transactions.";
                return RedirectToAction(nameof(Index));
            }

            if (!categories.Any())
            {
                TempData["Error"] = "Please create a category first before creating transactions.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Accounts = accounts;
            ViewBag.Categories = categories;
            return View();
        }
        catch (Exception ex)
        {
            // Log the exception
            return RedirectToAction(nameof(Index));
        }
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
        ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountName", transaction.AccountID);
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", transaction.CategoryId);
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
        ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountName", transaction.AccountID);
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", transaction.CategoryId);
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
        ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountName", transaction.AccountID);
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", transaction.CategoryId);
        return View(transaction);
    }

    // GET: Transactions/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transaction = await _context.Transactions
            .Include(t => t.Account)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.TransactionID == id);
        if (transaction == null)
        {
            return NotFound();
        }

        return View(transaction);
    }

    // POST: Transactions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TransactionExists(int id)
    {
        return _context.Transactions.Any(e => e.TransactionID == id);
    }
} 