using NextStep.Domain.Enums;

namespace NextStep.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; private set; } = string.Empty;
    public string? Name { get; set; }
    public string? CurrentJob { get; set; }
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Journey> Journeys { get; set; } = new List<Journey>();
    public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

    public Journey? GetActiveJourney() =>
        Journeys.FirstOrDefault(j => j.Status == JourneyStatus.Active);

    public void SetEmail(string email)
    {
        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        UpdatedAt = DateTime.UtcNow;
    }
}
