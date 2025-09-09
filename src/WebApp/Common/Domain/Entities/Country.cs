namespace Domain.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Language { get; set; }

        public ICollection<Region> Regions { get; set; } = [];
    }
}
