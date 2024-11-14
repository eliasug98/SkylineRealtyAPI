using Microsoft.EntityFrameworkCore;
using SkylineRealty.API.DBContext;
using SkylineRealty.API.Entities;
using SkylineRealty.API.Services.Interfaces;

namespace SkylineRealty.API.Services.Implementations
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly SkylineRealtyContext _context;
        public CommentsRepository(SkylineRealtyContext context)
        {
            _context = context;
        }

        public Comment? GetCommentById(int id)
        {
            return _context.Comments.Include(c => c.User).FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Comment>? GetCommentsByPropertyId(int idProperty)
        {
            return _context.Comments.Include(c => c.User).Where(p => p.PropertyId == idProperty);
        }

        public IEnumerable<Comment>? GetComments()
        {
            return _context.Comments.Include(c => c.User);
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
        }

        public bool CanUserComment(int userId, int propertyId)
        {
            var commentCount = _context.Comments
                .Count(c => c.UserId == userId && c.PropertyId == propertyId && c.CreatedDate.Date == DateTime.Now.Date);

            return commentCount < 1; // Permitir solo 1 comentario por día por propiedad
        }

        public void DeleteComment(Comment commentToDelete)
        {
            var comment = _context.Comments.FirstOrDefault(p => p.Id == commentToDelete.Id);
            if (comment != null)
            {
                _context.Comments.Remove(commentToDelete);
            }

        }

        public void Update(Comment comment)
        {
            _context.Comments.Update(comment);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

    }
}
