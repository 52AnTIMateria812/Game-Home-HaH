public class GameSession
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int GameId { get; set; }
    public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; }
} 