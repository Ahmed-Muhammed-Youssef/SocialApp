namespace Domain.Entities;

public class Connection
{
    public Connection() { }
    public Connection(string connectionId, int userId)
    {
        ConnectionId = connectionId;
        UserId = userId;
    }
    public string ConnectionId { get; set; } = string.Empty;
    public int UserId { get; set; }
}