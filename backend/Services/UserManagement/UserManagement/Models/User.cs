using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models;

public enum UserRole
{
    User = 0,
    Admin = 1
}

public class User
{
    [Key] public Guid Id = Guid.NewGuid();
    
    [Required]
    public string Name { get; set; } = "";
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
    
    public byte[]? PasswordHash { get; set; }
    
    public byte[]? PasswordSalt { get; set; }

    public UserRole Role { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime ModifiedAt { get; set; } = DateTime.Now;
}