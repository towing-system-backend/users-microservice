using Application.Core;

namespace User.Domain
{
    public class UserName : IValueObject<UserName>
    {
        private readonly string _value;

        public UserName(string value)
        {
            if (value.Length < 5 || value.Length > 20)
            {
                throw new InvalidUserNameException();
            }

            _value = value;
        }

        public string GetValue() => _value;
        public bool Equals(UserName other) => _value == other._value;
    }
}