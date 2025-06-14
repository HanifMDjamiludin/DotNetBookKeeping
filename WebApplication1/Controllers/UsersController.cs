using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.Entities;

namespace WebApplication1.Controllers
{
  public class UsersController : Controller
  {
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET: Users
    public async Task<IActionResult> Index()
    {
      var users = await _context.Users
        .Include(u => u.Accounts)
        .ToListAsync();
      return View(users);
    }

    // GET: Users/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var user = await _context.Users
        .Include(u => u.Accounts)
        .FirstOrDefaultAsync(u => u.UserId == id);

      if (user == null)
      {
        return NotFound();
      }

      return View(user);
    }

    // GET: Users/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Email")] User user)
    {
      if (ModelState.IsValid)
      {
        _context.Add(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(user);
    }

    // GET: Users/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var user = await _context.Users.FindAsync(id);
      if (user == null)
      {
        return NotFound();
      }
      return View(user);
    }

    // POST: Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,Email")] User user)
    {
      if (id != user.UserId)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(user);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!UserExists(user.UserId))
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
      return View(user);
    }

    // GET: Users/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var user = await _context.Users
        .FirstOrDefaultAsync(u => u.UserId == id);
      if (user == null)
      {
        return NotFound();
      }

      return View(user);
    }

    // POST: Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var user = await _context.Users.FindAsync(id);
      if (user != null)
      {
        _context.Users.Remove(user);
      }
      
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool UserExists(int id)
    {
      return _context.Users.Any(e => e.UserId == id);
    }
  }
}
