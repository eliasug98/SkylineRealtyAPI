using SkylineRealty.API.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkylineRealty.API.DTOs.PropertyDTOs
{
    public class PropertyDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Address { get; set; }

        public decimal Price { get; set; }

        public List<string> Images { get; set; }

        public string Description { get; set; }

        public int Beds { get; set; }

        public int Baths { get; set; }

        public int Garages { get; set; }

        public int Sqft { get; set; }

        public DateTime CreatedDate { get; set; }

        public int SellerId { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
