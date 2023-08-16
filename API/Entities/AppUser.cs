using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
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
        public ICollection<Picture> Photos { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }

        // Collection navigation properties
        public ICollection<AppUserRole> UserRoles { get; set; }
        [InverseProperty(nameof(FriendRequest.Requister))]
        public ICollection<FriendRequest> LikesLikers { get; set; }
        [InverseProperty(nameof(FriendRequest.Requisted))]
        public ICollection<FriendRequest> LikesLikees { get; set; }
        [InverseProperty(nameof(Friend.User))]
        public ICollection<Friend> MatchesId { get; set; }
        [InverseProperty(nameof(Friend.FriendUser))]
        public ICollection<Friend> MatchesMatchedId { get; set; }
    }
}
