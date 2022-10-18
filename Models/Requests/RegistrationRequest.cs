namespace HelloApi.Models.Requests
{
    public class RegistrationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public int RoleId { get; set; }
        public string BirthDate { get; set; }
    }
}
