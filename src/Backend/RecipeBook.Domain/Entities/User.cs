namespace RecipeBook.Domain.Entities;

public class User
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}