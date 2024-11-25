using Application.Core;

namespace User.Domain
{
    public class UserIdentificationNumber : IValueObject<UserIdentificationNumber>
    {
        private readonly int Value;

        public UserIdentificationNumber(int value)
        {
            if (value < 999999 || value > 40000000)
            {
                throw new InvalidUserIdentificationNumberException();
            }

            Value = value;
        }

        public int GetValue() => Value;
        public bool Equals(UserIdentificationNumber other) => Value == other.Value;
    }
}