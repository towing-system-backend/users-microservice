using Application.Core;

namespace User.Domain
{
    public class UserRole : IValueObject<UserRole>
    {
        private readonly string _value;
        private static readonly string[] ValidRoles = { "Admin", "Provider", "TowDriver", "Employee", "CabinOperator" };

        public UserRole(string value)
        {
            if (!IsValidRole(value))
            {
                throw new InvalidUserRoleException();
            }

            _value = value;
        }

        private static bool IsValidRole(string value)
        {
            return Array.Exists(ValidRoles, role => role.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        public string GetValue() => _value;
        public bool Equals(UserRole other) => _value == other._value;
    }
}
