using System.ComponentModel.DataAnnotations;

namespace SV.Demo.Application.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime LastUpdated { get; set; }
}