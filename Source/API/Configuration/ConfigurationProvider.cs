namespace API.Configuration
{
    public static class ConfigurationProvider
    {
        public static IConfiguration GetConfig(IConfiguration configuration)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}
