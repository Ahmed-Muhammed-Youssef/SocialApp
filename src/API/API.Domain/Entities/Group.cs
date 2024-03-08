namespace API.Domain.Entities
{
    public class Group
    {
        public Group() { }
        public Group(string name) { Name = name; }
        public string Name { get; set; }

        // navigation properties
        public ICollection<Connection> Connections { get; set; } = [];

    }
}