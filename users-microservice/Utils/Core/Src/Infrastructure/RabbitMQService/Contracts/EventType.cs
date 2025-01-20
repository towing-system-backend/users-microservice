namespace RabbitMQ.Contracts
{
    public interface IRabbitMQMessage { };

    public record EventType(
        string PublisherId,
        string Type,
        object Context,
        DateTime OcurredDate
    );

    public record CreateUser(
        string Id,
        string SupplierCompanyId,
        string Name,
        string Image,
        string Email,
        string Role,
        string Status,
        string PhoneNumber,
        int IdentificationNumber
    ) : IRabbitMQMessage;
}