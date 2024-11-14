using SkylineRealty.API.Entities;

namespace SkylineRealty.API.Services.Interfaces
{
    public interface IMessagesRepository
    {
        void AddMessage(Message message);
        IEnumerable<Message> GetMessagesByUserId(int userId);
        IEnumerable<Message> GetMessages();
        bool MessageExists(int id);
        void SaveChanges();
        IEnumerable<Message> GetUnreadAdminMessages(int userId);
        IEnumerable<Message> GetUnreadUserMessages(int userId);
    }
}
