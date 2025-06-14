using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Entities;

namespace WebApplication1.Controllers;

public class CategoriesController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Categories
    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
            .Include(c => c.ParentCategory)
            .Include(c => c.SubCategories)
            .ToListAsync();
        return View(categories);
    }

    // GET: Categories/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories
            .Include(c => c.ParentCategory)
            .Include(c => c.SubCategories)
            .Include(c => c.Transactions)
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    // GET: Categories/Create
    public async Task<IActionResult> Create()
    {
        ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
        return View();
    }

    // POST: Categories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description,Type,ParentCategoryId")] Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", category.ParentCategoryId);
        return View(category);
    }

    // GET: Categories/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", category.ParentCategoryId);
        return View(category);
    }

    // POST: Categories/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name,Description,Type,ParentCategoryId")] Category category)
    {
        if (id != category.CategoryId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.CategoryId))
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
        ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", category.ParentCategoryId);
        return View(category);
    }

    // GET: Categories/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories
            .Include(c => c.ParentCategory)
            .FirstOrDefaultAsync(c => c.CategoryId == id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    // POST: Categories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(e => e.CategoryId == id);
    }
} 