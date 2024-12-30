using Application.Core;

namespace User.Domain
{
    public class UserStatus : IValueObject<UserStatus>
    {
        private readonly string _value;
        private static readonly string[] ValidStatuses = { "Active", "Inactive" };

        public UserStatus(string value)
        {
            if (!IsValidStatus(value))
            {
                throw new InvalidUserStatusException();
            }

            _value = value;
        }

        private static bool IsValidStatus(string value)
        {
            return Array.Exists(ValidStatuses, status => status.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        public string GetValue() => _value;
        public bool Equals(UserStatus other) => _value == other._value;
    }
}
