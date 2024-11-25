using Application.Core;

namespace User.Domain
{
    public class UserName : IValueObject<UserName>
    {
        private readonly string Value;

        public UserName(string value)
        {
            if (value.Length < 5 || value.Length > 20)
            {
                throw new InvalidUserNameException();
            }

            Value = value;
        }

        public string GetValue() => Value;
        public bool Equals(UserName other) => Value == other.Value;
    }
}