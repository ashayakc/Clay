namespace API.Configuration
{
    public class AppConfig
    {
        public AppConfig(IConfiguration configuration)
        {
            configuration.Bind(this);
        }

        public string DbConnectionString { get; set; }
        public string ElasticUrl { get; set; }
        public string IndexName { get; set; }
        public string ElasticUserName { get; set; }
        public string ElasticPassword { get; set; }
    }
}
