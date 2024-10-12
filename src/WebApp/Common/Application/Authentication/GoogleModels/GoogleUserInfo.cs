﻿namespace Application.Authentication.GoogleModels
{
    public class GoogleUserInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool VerifiedEmail { get; set; }
        public string PictureUrl { get; set; }
    }
}
