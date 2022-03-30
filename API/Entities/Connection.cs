namespace API.Entities
{
    public class Connection
    {
        public Connection()
        {
            
        }
        public Connection(string connectionId, int userId) 
        {
            this.ConnectionId = connectionId;
            this.UserId = userId;
        }
        
        public string ConnectionId { get; set; }
        public int UserId { get; set; }
    }
}