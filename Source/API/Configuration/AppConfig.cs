namespace API.Configuration
{
    public class AppConfig
    {
        public AppConfig(IConfiguration configuration)
        {
            configuration.Bind(this);
        }

        public string DbConnectionString { get; set; }
    }
}
