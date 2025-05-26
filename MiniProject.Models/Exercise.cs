using System.Text.Json.Serialization;

namespace MiniProject.Models;
public class Exercise
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
}