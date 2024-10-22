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
            return _context.Comments.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Comment>? GetCommentsByPropertyId(int idProperty)
        {
            return _context.Comments.Where(p => p.PropertyId == idProperty);
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
        }

        public void DeleteComment(Comment commentToDelete)
        {
            var comment = _context.Comments.FirstOrDefault(p => p.Id == commentToDelete.Id);
            if (comment != null)
            {
                _context.Comments.Remove(commentToDelete);
            }

        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

    }
}
