namespace test.Models;

public class LoginHistory
{
    public int LoginHistoryId { get; set; }

    public int? UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime LoggedInAtUtc { get; set; }
}
