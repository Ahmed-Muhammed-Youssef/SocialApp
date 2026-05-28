using System;
using System.Collections.Generic;
using System.Text;

namespace API.Test.Infrastructure;

internal static class Routes
{
    internal static class Auth 
    {
        public const string Register = "/api/auth/register";
        public const string Login = "/api/auth/login";
        public const string GoogleSignIn = "/api/auth/google-signin";
    }

    internal static class Users
    {
        public static string GetById(int id) => $"/api/users/{id}";
    }
}
