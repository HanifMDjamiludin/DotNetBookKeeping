using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities;

[Table("accounts")]
public class Account 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("account_id")]
    public int AccountID { get; set; }

    [Required]
    [Column("account_name")]
    [StringLength(100)]
    public string AccountName { get; set; }

    [Required]
    [Column("balance", TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public void AddTransaction(Transaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));
            
        Transactions.Add(transaction);
        Balance += transaction.Amount;
    }
} 