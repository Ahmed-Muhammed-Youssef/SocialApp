namespace Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Foreign Key
        public int RegionId { get; set; }
    }
}
