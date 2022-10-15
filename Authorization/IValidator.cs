namespace HelloApi.Authorization
{
    public interface IValidator
    {
        public bool ValidateEmail(string email);

        public bool ValidatePassword(string password);
    }
}
