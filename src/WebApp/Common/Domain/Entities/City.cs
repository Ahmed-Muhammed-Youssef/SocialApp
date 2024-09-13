namespace Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AName { get; set; }

        // Foreign Key
        public int CountryId { get; set; }
    }
}
