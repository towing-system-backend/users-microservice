using Application.Core;

namespace User.Domain
{
    public class InvalidSupplierCompanyIdException : DomainException
    {
        public InvalidSupplierCompanyIdException() : base("Invalid supplier company id.") { }
    }
}
