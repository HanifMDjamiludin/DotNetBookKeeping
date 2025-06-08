using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities;

[Table("transactions")]
public class Transaction 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("transaction_id")]
    public int TransactionID { get; set; }

    [Required]
    [Column("date")]
    public DateTime Date { get; set; }

    [Required]
    [Column("description")]
    [StringLength(255)]
    public string Description { get; set; }

    [Required]
    [Column("amount", TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    [Column("account_id")]
    public int AccountID { get; set; }

    [ForeignKey("AccountID")]
    public virtual Account Account { get; set; }

    [Column("category_id")]
    public int? CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; }

    [Required]
    [Column("type")]
    public TransactionType Type { get; set; }

    [Column("is_recurring")]
    public bool IsRecurring { get; set; }

    [Column("recurrence_interval")]
    public RecurrenceInterval? RecurrenceInterval { get; set; }

    // Parameterless constructor for EF Core
    public Transaction()
    {
    }

    public Transaction(DateTime date, string description, decimal amount, int accountId)
    {
        Date = date;
        Description = description;
        Amount = amount;
        AccountID = accountId;
    }
} 