using Microsoft.EntityFrameworkCore;
using SkylineRealty.API.DBContext;
using SkylineRealty.API.Entities;
using SkylineRealty.API.Services.Interfaces;

namespace SkylineRealty.API.Services.Implementations
{
    public class PropertiesRepository : IPropertiesRepository
    {
        private readonly SkylineRealtyContext _context;

        public PropertiesRepository(SkylineRealtyContext context)
        {
            _context = context;
        }

        public IEnumerable<Property> GetProperties()
        {
            // Incluir Images y Comments en la consulta
            return _context.Properties
                .Include(p => p.User)
                .Include(p => p.Images)
                .Include(p => p.Comments);
        }

        public Property? GetPropertyById(int idProperty)
        {
            // Incluir Images y Comments en la consulta por ID
            return _context.Properties
                .Include(p => p.User)
                .Include(p => p.Images)
                .Include(p => p.Comments)
                .FirstOrDefault(p => p.Id == idProperty);
        }

        public void AddProperty(Property property)
        {
            _context.Properties.Add(property);
        }

        public void DeleteProperty(Property propertyToDelete)
        {
            var property = _context.Properties.Include(p => p.Images).Include(p => p.Comments).FirstOrDefault(p => p.Id == propertyToDelete.Id);
            if (property != null)
            {
                _context.Images.RemoveRange(property.Images);
                _context.Comments.RemoveRange(property.Comments);
                _context.Properties.Remove(propertyToDelete);
            }
        }

        public bool PropertyExists(string title)
        {
            return _context.Properties.Any(o => o.Title == title);
        }

        public void Update(Property property)
        {
            _context.Properties.Update(property);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<Property> GetPropertiesBySellerId(int sellerId)
        {
            // Incluir Images y Comments en la consulta por SellerId
            return _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Comments)
                .Where(p => p.SellerId == sellerId);
        }
    }
}
