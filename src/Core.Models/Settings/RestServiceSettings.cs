namespace Core.Models.Settings
{
    public class RestServiceSettings
    {
        public Service DriverApi { get; set; }

        public class Service
        {
            public string Host { get; set; }
            public string Version { get; set; }
        }
    }
}