using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class IdPairs
    {
        [Required]
        public int FirstId { get; set; }
        [Required]
        public int SecondId { get; set; }
    }
}
