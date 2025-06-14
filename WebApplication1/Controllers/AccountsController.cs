using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Entities;

namespace WebApplication1.Controllers;

public class AccountsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Accounts
    public async Task<IActionResult> Index()
    {
        var accounts = await _context.Accounts
            .Include(a => a.User)
            .Include(a => a.Transactions)
            .ToListAsync();
        return View(accounts);
    }

    // GET: Accounts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var account = await _context.Accounts
            .Include(a => a.User)
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.AccountID == id);

        if (account == null)
        {
            return NotFound();
        }

        return View(account);
    }

    // GET: Accounts/Create
    public async Task<IActionResult> Create()
    {
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name");
        return View();
    }

    // POST: Accounts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AccountName,Balance,UserId")] Account account)
    {
        if (ModelState.IsValid)
        {
            _context.Add(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", account.UserId);
        return View(account);
    }

    // GET: Accounts/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var account = await _context.Accounts.FindAsync(id);
        if (account == null)
        {
            return NotFound();
        }
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", account.UserId);
        return View(account);
    }

    // POST: Accounts/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("AccountID,AccountName,Balance,UserId")] Account account)
    {
        if (id != account.AccountID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(account);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(account.AccountID))
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
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", account.UserId);
        return View(account);
    }

    // GET: Accounts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var account = await _context.Accounts
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.AccountID == id);
        if (account == null)
        {
            return NotFound();
        }

        return View(account);
    }

    // POST: Accounts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var account = await _context.Accounts.FindAsync(id);
        if (account != null)
        {
            _context.Accounts.Remove(account);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AccountExists(int id)
    {
        return _context.Accounts.Any(e => e.AccountID == id);
    }
} 