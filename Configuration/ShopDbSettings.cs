namespace ShopApi.Configuration
{
    public class ShopDbSettings
    {
        public const string SectionName = "ShopDbSettings";

        public bool InitData { get; set; }

        public MainAdminSettings MainAdmin { get; set; }
    }

    public class MainAdminSettings
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
