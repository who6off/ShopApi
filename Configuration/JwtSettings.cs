namespace ShopApi.Configuration
{
	public class JwtSettings
	{
		public const string SectionName = "Jwt";
		public string Key { get; set; } = String.Empty;
		public string Issuer { get; set; } = String.Empty;
		public string Audience { get; set; } = String.Empty;
		public string Subject { get; set; } = String.Empty;
		public int ValidHours { get; set; }
	}
}
