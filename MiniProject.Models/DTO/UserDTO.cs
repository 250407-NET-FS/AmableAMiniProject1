using System.ComponentModel.DataAnnotations;

namespace MiniProject.Models.DTOs;

public class UserDTO
{

    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public bool IsLockedOut { get; set; }
    public string? Role { get; set; }

}