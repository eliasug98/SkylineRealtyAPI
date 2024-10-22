using SkylineRealty.API.Entities;

namespace SkylineRealty.API.Services.Interfaces
{
    public interface ICommentsRepository
    {
        void AddComment(Comment comment);
        void DeleteComment(Comment commentToDelete);
        Comment? GetCommentById(int id);
        IEnumerable<Comment>? GetCommentsByPropertyId(int idProperty);
        bool SaveChanges();
    }
}
