using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities;

[Table("budgets")]
public class Budget
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("budget_id")]
    public int BudgetId { get; set; }

    [Required]
    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Required]
    [Column("end_date")]
    public DateTime EndDate { get; set; }

    [Required]
    [Column("planned_amount", TypeName = "decimal(18,2)")]
    public decimal PlannedAmount { get; set; }

    [Required]
    [Column("category_id")]
    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; }
    
    [Column("actual_amount", TypeName = "decimal(18,2)")]
    public decimal ActualAmount { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }
} 