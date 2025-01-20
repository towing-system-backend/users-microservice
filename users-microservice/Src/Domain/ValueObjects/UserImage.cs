using Application.Core;

namespace User.Domain
{
    public class UserImage : IValueObject<UserImage>
    {
        private readonly string _value;

        public UserImage(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidUserImageException();
            }

            _value = value;
        }

        public string GetValue() => _value;
        public bool Equals(UserImage other) => _value == other._value;
    }
}
