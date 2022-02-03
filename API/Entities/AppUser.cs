using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; } // this convention names helps with the entity framework.
        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }
        [Required]
        public Sex Sex { get; set; }
        [Required]
        public Interest Interest { get; set; }
        // Collection navigation properties
        [InverseProperty(nameof(Like.Liker))]
        public ICollection<Like> LikesLikers { get; set; }
        [InverseProperty(nameof(Like.Liked))]
        public ICollection<Like> LikesLikees { get; set; }
        [InverseProperty(nameof(Match.User))]
        public ICollection<Match> MatchesId { get; set; }
        [InverseProperty(nameof(Match.Matched))]
        public ICollection<Match> MatchesMatchedId { get; set; }

    }
    public enum Sex
    {
        M,
        F
    }
    public enum Interest
    {
        M,
        F,
        Both
    }
}
