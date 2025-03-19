using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities;

[Table("user_account")]

public class UserAccount
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  [Column("id")]
  public int Id { get; set; }

  [Column("full_name")]
  public string? FullName { get; set; }

  [Column("user_name")]
  public string? UserName { get; set; }
  
  [Column("password")]
  public string? Password { get; set; }

}


