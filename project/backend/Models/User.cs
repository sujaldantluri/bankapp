using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public decimal Checking { get; set; } // Add Checking property
    public decimal Savings { get; set; } // Add Savings property
}
