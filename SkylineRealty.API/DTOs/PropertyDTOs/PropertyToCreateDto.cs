using SkylineRealty.API.DTOs.ImageDTOs;

namespace SkylineRealty.API.DTOs.PropertyDTOs
{
    public class PropertyToCreateDto
    {
        public string Title { get; set; }

        public string Address { get; set; }

        public decimal Price { get; set; }

        public ICollection<ImageDto> Images { get; set; } = new List<ImageDto>();

        public string Description { get; set; }

        public int Beds { get; set; }

        public int Baths { get; set; }

        public int Garages { get; set; }

        public int Sqft { get; set; }

        public int SellerId { get; set; }
    }
}
