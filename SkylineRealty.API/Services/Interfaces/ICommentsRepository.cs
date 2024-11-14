using SkylineRealty.API.Entities;

namespace SkylineRealty.API.Services.Interfaces
{
    public interface ICommentsRepository
    {
        void AddComment(Comment comment);
        bool CanUserComment(int userId, int propertyId);
        void DeleteComment(Comment commentToDelete);
        Comment? GetCommentById(int id);
        IEnumerable<Comment>? GetComments();
        IEnumerable<Comment>? GetCommentsByPropertyId(int idProperty);
        bool SaveChanges();
        void Update(Comment comment);
    }
}
