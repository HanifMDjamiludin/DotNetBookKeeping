using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities;

[Table("categories")]
public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("category_id")]
    public int CategoryId { get; set; }

    [Required]
    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; }

    [Column("description")]
    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    [Column("type")]
    public CategoryType Type { get; set; }

    [Column("parent_category_id")]
    public int? ParentCategoryId { get; set; }

    [ForeignKey("ParentCategoryId")]
    public virtual Category ParentCategory { get; set; }

    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
    
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
} 