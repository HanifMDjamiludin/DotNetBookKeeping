using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities;

[Table("users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("user_id")]
    public int UserId { get; set; }

    [Required]
    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [Column("email")]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; }    

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}

[Table("business_users")]
public class BusinessUser : User
{
    [Column("tax_rate")]
    public decimal TaxRate { get; set; }

    public decimal CalculateTax(decimal amount)
    {
        return amount * (TaxRate / 100);
    }
} 