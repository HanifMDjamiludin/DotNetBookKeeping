using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Entities;

namespace WebApplication1.Controllers;

public class TestDataController : Controller
{
    private readonly ApplicationDbContext _context;

    public TestDataController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: TestData/CreateSampleData
    public async Task<IActionResult> CreateSampleData()
    {
        // 1. Create Users
        var john = new User 
        { 
            Name = "John Doe",
            Email = "john@example.com"
        };
        var jane = new User
        {
            Name = "Jane Smith",
            Email = "jane@example.com"
        };
        _context.Users.AddRange(john, jane);
        await _context.SaveChangesAsync();

        // 2. Create Accounts
        var johnChecking = new Account
        {
            AccountName = "John's Checking",
            Balance = 1000.00M,
            UserId = john.UserId
        };
        var johnSavings = new Account
        {
            AccountName = "John's Savings",
            Balance = 5000.00M,
            UserId = john.UserId
        };
        var janeChecking = new Account
        {
            AccountName = "Jane's Checking",
            Balance = 2500.00M,
            UserId = jane.UserId
        };
        _context.Accounts.AddRange(johnChecking, johnSavings, janeChecking);
        await _context.SaveChangesAsync();

        // 3. Create Transactions
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Date = DateTime.UtcNow.AddDays(-5),
                Description = "Monthly Salary",
                Amount = 5000.00M,
                AccountID = johnChecking.AccountID,
                CategoryId = 1, // Salary
                Type = TransactionType.Income,
                IsRecurring = true,
                RecurrenceInterval = RecurrenceInterval.Monthly
            },
            new Transaction
            {
                Date = DateTime.UtcNow.AddDays(-3),
                Description = "Apartment Rent",
                Amount = -1500.00M,
                AccountID = johnChecking.AccountID,
                CategoryId = 2, // Rent
                Type = TransactionType.Expense,
                IsRecurring = true,
                RecurrenceInterval = RecurrenceInterval.Monthly
            },
            new Transaction
            {
                Date = DateTime.UtcNow.AddDays(-2),
                Description = "Grocery Shopping",
                Amount = -150.00M,
                AccountID = johnChecking.AccountID,
                CategoryId = 3, // Groceries
                Type = TransactionType.Expense
            },
            new Transaction
            {
                Date = DateTime.UtcNow.AddDays(-1),
                Description = "Utility Bills",
                Amount = -200.00M,
                AccountID = johnChecking.AccountID,
                CategoryId = 4, // Utilities
                Type = TransactionType.Expense,
                IsRecurring = true,
                RecurrenceInterval = RecurrenceInterval.Monthly
            }
        };
        _context.Transactions.AddRange(transactions);

        // 4. Create Budgets
        var budgets = new List<Budget>
        {
            new Budget
            {
                Name = "Monthly Rent Budget",
                StartDate = new DateTime(2024, 4, 1),
                EndDate = new DateTime(2024, 4, 30),
                PlannedAmount = 1500.00M,
                ActualAmount = 1500.00M,
                CategoryId = 2, // Rent
                UserId = john.UserId
            },
            new Budget
            {
                Name = "Monthly Groceries Budget",
                StartDate = new DateTime(2024, 4, 1),
                EndDate = new DateTime(2024, 4, 30),
                PlannedAmount = 600.00M,
                ActualAmount = 150.00M, // So far
                CategoryId = 3, // Groceries
                UserId = john.UserId
            },
            new Budget
            {
                Name = "Monthly Utilities Budget",
                StartDate = new DateTime(2024, 4, 1),
                EndDate = new DateTime(2024, 4, 30),
                PlannedAmount = 250.00M,
                ActualAmount = 200.00M,
                CategoryId = 4, // Utilities
                UserId = john.UserId
            }
        };
        _context.Budgets.AddRange(budgets);
        await _context.SaveChangesAsync();

        return Content("Sample data created successfully!");
    }
} 