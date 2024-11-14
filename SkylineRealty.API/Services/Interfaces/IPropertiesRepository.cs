using SkylineRealty.API.Entities;

namespace SkylineRealty.API.Services.Interfaces
{
    public interface IPropertiesRepository
    {
        void AddProperty(Property property);
        void DeleteProperty(Property propertyToDelete);
        IEnumerable<Property> GetProperties();
        IEnumerable<Property> GetPropertiesBySellerId(int sellerId);
        Property? GetPropertyById(int idProperty);
        bool PropertyExists(string title);
        bool SaveChanges();
        void Update(Property property);
    }
}
