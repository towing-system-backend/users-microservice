using Application.Core;

namespace User.Domain
{
    public class UserEmail : IValueObject<UserEmail>
    {
        private readonly string Value;

        public UserEmail(string value)
        {
            if (!EmailRegex.IsEmail(value))
            {
                throw new InvalidUserEmailException();
            }

            Value = value;
        }

        public string GetValue() => Value;
        public bool Equals(UserEmail other) => Value == other.Value;
    }
}