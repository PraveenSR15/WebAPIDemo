namespace WebAPIDemo.Authority
{
    public class AppRepository
    {
        private static List<Application> applications = new List<Application>
        {
            new Application
            {
                ApplicationId = 1,
                ApplicationName = "MyWebApp",
                ClientId = "E551DE03-F774-4FD5-81ED-81CBCC3C5A0A",
                Secret = "E5ABCED0-48F2-4CA3-839D-C15B4CBF64A4",
                Scopes = "read,write"
            }
        };

        public static bool Authenticate(string clientId, string secret)
        {
            return applications.Any(x => x.ClientId == clientId && x.Secret == secret);
        }

        public static Application? GetApplication(string clientId)
        {
            return applications.FirstOrDefault(x => x.ClientId == clientId);
        }
    }
}
