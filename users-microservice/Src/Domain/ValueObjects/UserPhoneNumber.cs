using Application.Core;

namespace User.Domain
{
    public class UserPhoneNumber : IValueObject<UserPhoneNumber>
    {
        private readonly string _value;

        public UserPhoneNumber(string value)
        {
            if (!PhoneNumberRegex.IsPhoneNumber(value))
            {
                throw new InvalidUserPhoneNumberException();
            }

            _value = value;
        }

        public string GetValue() => _value;
        public bool Equals(UserPhoneNumber other) => _value == other._value;
    }
}