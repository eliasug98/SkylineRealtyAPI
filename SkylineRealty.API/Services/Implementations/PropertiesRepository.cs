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
            return _context.Properties;
        }

        public Property? GetPropertyById(int idProperty)
        {
            return _context.Properties.Where(p => p.Id == idProperty).FirstOrDefault();
        }

        public void AddProperty(Property property)
        {
            _context.Properties.Add(property);
        }

        public void DeleteProperty(Property propertyToDelete)
        {
            var property = _context.Properties.FirstOrDefault(p => p.Id == propertyToDelete.Id);
            if (property != null)
            {
                _context.Properties.Remove(propertyToDelete);
            }

        }

        public bool PropertyExists(string title)
        {
            return _context.Properties.Where(o => o.Title == title).Any();
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
            return _context.Properties.Where(p => p.SellerId == sellerId);
        }
    }
}
