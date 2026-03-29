namespace RealEstateApp.Application.Interfaces
{
    public interface INotificationClient
    {
        Task ReceiveNotification(object notification);
    }
}