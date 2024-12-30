using Application.Core;

namespace User.Domain
{
    public class UserEmail : IValueObject<UserEmail>
    {
        private readonly string _value;

        public UserEmail(string value)
        {
            if (!EmailRegex.IsEmail(value))
            {
                throw new InvalidUserEmailException();
            }

            _value = value;
        }

        public string GetValue() => _value;
        public bool Equals(UserEmail other) => _value == other._value;
    }
}