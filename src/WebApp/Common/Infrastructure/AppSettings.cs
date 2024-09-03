namespace Infrastructure
{
    internal static class AppSettings
    {
        public static string DefaultConnectionString { get; } = "Server=AHMED-YOUSSEF;Database=SocialApp;Trusted_Connection=True;TrustServerCertificate=True";
        public static string IdentityConnectionString { get; } = "Server=AHMED-YOUSSEF;Database=SocialAppIdentity;Trusted_Connection=True;TrustServerCertificate=True";
    }
}
