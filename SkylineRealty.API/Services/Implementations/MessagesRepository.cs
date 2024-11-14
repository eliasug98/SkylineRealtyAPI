using SkylineRealty.API.DBContext;
using SkylineRealty.API.Entities;
using SkylineRealty.API.Services.Interfaces;

namespace SkylineRealty.API.Services.Implementations
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly SkylineRealtyContext _context;
        public MessagesRepository(SkylineRealtyContext context)
        {
            _context = context;
        }

        public IEnumerable<Message> GetMessages()
        {
            return _context.Messages;
        }

        public IEnumerable<Message> GetUnreadAdminMessages(int userId)
        {
            return _context.Messages.Where(m => m.UserId == userId && !m.IsRead && m.AdminId != 0);
        }

        public IEnumerable<Message> GetUnreadUserMessages(int userId)
        {
            return _context.Messages.Where(m => m.UserId == userId && !m.IsRead && m.AdminId == 0);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Message> GetMessagesByUserId(int userId)
        {
            return _context.Messages.Where(m => m.UserId == userId);
        }

    }
}
