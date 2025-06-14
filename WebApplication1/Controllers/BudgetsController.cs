using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Entities;

namespace WebApplication1.Controllers;

public class BudgetsController : Controller
{
    private readonly ApplicationDbContext _context;

    public BudgetsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Budgets
    public async Task<IActionResult> Index()
    {
        var budgets = await _context.Budgets
            .Include(b => b.Category)
            .Include(b => b.User)
            .ToListAsync();
        return View(budgets);
    }

    // GET: Budgets/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var budget = await _context.Budgets
            .Include(b => b.Category)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.BudgetId == id);

        if (budget == null)
        {
            return NotFound();
        }

        return View(budget);
    }

    // GET: Budgets/Create
    public async Task<IActionResult> Create()
    {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name");
        return View();
    }

    // POST: Budgets/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Amount,StartDate,EndDate,CategoryId,UserId")] Budget budget)
    {
        if (ModelState.IsValid)
        {
            _context.Add(budget);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", budget.CategoryId);
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", budget.UserId);
        return View(budget);
    }

    // GET: Budgets/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var budget = await _context.Budgets.FindAsync(id);
        if (budget == null)
        {
            return NotFound();
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", budget.CategoryId);
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", budget.UserId);
        return View(budget);
    }

    // POST: Budgets/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("BudgetId,Amount,StartDate,EndDate,CategoryId,UserId")] Budget budget)
    {
        if (id != budget.BudgetId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(budget);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetExists(budget.BudgetId))
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
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", budget.CategoryId);
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", budget.UserId);
        return View(budget);
    }

    // GET: Budgets/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var budget = await _context.Budgets
            .Include(b => b.Category)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.BudgetId == id);
        if (budget == null)
        {
            return NotFound();
        }

        return View(budget);
    }

    // POST: Budgets/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var budget = await _context.Budgets.FindAsync(id);
        if (budget != null)
        {
            _context.Budgets.Remove(budget);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BudgetExists(int id)
    {
        return _context.Budgets.Any(e => e.BudgetId == id);
    }
} 