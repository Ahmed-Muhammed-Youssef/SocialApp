using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserDTO
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(255)]
        public string Username { get; set; }
        [Required, MaxLength(255)]
        public string FirstName { get; set; }
        [Required, MaxLength(255)]
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        [Required]
        public char Sex { get; set; }
        [Required]
        public char Interest { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime LastActive { get; set; }
        [Required]
        public string Bio { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }

        public IEnumerable<PictureDTO> Pictures { get; set; }
    }
}
