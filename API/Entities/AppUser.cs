using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; } // this convention names helps with the entity framework.
        [Required]
        [MaxLength(255)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }
        [Required]
        public char Sex { get; set; }
        [Required]
        public char Interest { get; set; } // can be f (female), m (male) or b (both)
        [Required]
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Bio { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }


        // Credentials
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public byte[] Password { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }

        // Collection navigation properties
        
        [InverseProperty(nameof(Like.Liker))]
        public ICollection<Like> LikesLikers { get; set; }
        [InverseProperty(nameof(Like.Liked))]
        public ICollection<Like> LikesLikees { get; set; }
        [InverseProperty(nameof(Match.User))]
        public ICollection<Match> MatchesId { get; set; }
        [InverseProperty(nameof(Match.Matched))]
        public ICollection<Match> MatchesMatchedId { get; set; }

        /*public int GetAge() => DateOfBirth.CalculateAge();*/

    }
}
