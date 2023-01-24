using System.Text.RegularExpressions;

namespace ShopApi.Authentication
{
    public class Validator : IValidator
    {
        private readonly Regex _validEmailRegex;
        private readonly Regex _validPasswordPattern;
        public Validator()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            _validEmailRegex = new Regex(validEmailPattern, RegexOptions.IgnoreCase);

            string validPasswordPattern = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,15}$";
            _validPasswordPattern = new Regex(validPasswordPattern, RegexOptions.IgnoreCase);
        }

        public bool ValidateEmail(string email)
        {
            return _validEmailRegex.IsMatch(email);
        }

        public bool ValidatePassword(string password)
        {
            return _validPasswordPattern.IsMatch(password);
        }
    }
}
