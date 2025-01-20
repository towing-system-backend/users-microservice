using Application.Core;

namespace User.Domain
{
    public class UserIdentificationNumber : IValueObject<UserIdentificationNumber>
    {
        private readonly int _value;

        public UserIdentificationNumber(int value)
        {
            if (value < 999999 || value > 40000000)
            {
                throw new InvalidUserIdentificationNumberException();
            }

            _value = value;
        }

        public int GetValue() => _value;
        public bool Equals(UserIdentificationNumber other) => _value == other._value;
    }
}