namespace HelloApi.Configuration
{
    public static class ConfigurationExtensions
    {
        public static int GetAdultAge(this IConfiguration configuration)
        {
            return configuration.GetValue<int>("AdultAge");
        }
    }
}
