﻿using System.ComponentModel.DataAnnotations;
using Application.DTOs.Picture;

namespace Application.DTOs.User
{
    public class UserDTO
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(255)]
        public string FirstName { get; set; }
        [Required, MaxLength(255)]
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        [Required]
        public char Sex { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime LastActive { get; set; }
        [Required]
        public string Bio { get; set; }
        public IEnumerable<PictureDTO> Pictures { get; set; }
    }
}
