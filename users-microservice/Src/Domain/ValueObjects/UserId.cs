using Application.Core;

namespace User.Domain
{
    public class UserId : IValueObject<UserId>
    {
        private readonly string Value;

        public UserId(string value)
        {
            if (!GuidEx.IsGuid(value))
            {
                throw new InvalidUserIdException();
            }

            Value = value;
        }

        public string GetValue() => Value;
        public bool Equals(UserId other) => Value == other.Value;
    }
}