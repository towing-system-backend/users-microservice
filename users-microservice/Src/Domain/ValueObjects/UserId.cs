using Application.Core;

namespace User.Domain
{
    public class UserId : IValueObject<UserId>
    {
        private readonly string _value;

        public UserId(string value)
        {
            if (!GuidEx.IsGuid(value))
            {
                throw new InvalidUserIdException();
            }

            _value = value;
        }

        public string GetValue() => _value;
        public bool Equals(UserId other) => _value == other._value;
    }
}