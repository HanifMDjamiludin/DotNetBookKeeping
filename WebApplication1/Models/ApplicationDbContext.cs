using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.Entities;

namespace WebApplication1.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Budget> Budgets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships
        modelBuilder.Entity<User>()
            .HasMany(u => u.Accounts)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Account>()
            .HasMany(a => a.Transactions)
            .WithOne(t => t.Account)
            .HasForeignKey(t => t.AccountID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
            .HasMany(c => c.Transactions)
            .WithOne(t => t.Category)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        // Add some default categories
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Salary", Description = "Regular income from employment", Type = CategoryType.Income },
            new Category { CategoryId = 2, Name = "Rent", Description = "Monthly housing payment", Type = CategoryType.Expense },
            new Category { CategoryId = 3, Name = "Groceries", Description = "Food and household items", Type = CategoryType.Expense },
            new Category { CategoryId = 4, Name = "Utilities", Description = "Electricity, water, and other utilities", Type = CategoryType.Expense }
        );
    }
} 