namespace SkylineRealty.API.DTOs.PropertyDTOs
{
    public class PropertyToUpdateDto
    {
        public string Title { get; set; }

        public string Adress { get; set; }

        public decimal Price { get; set; }

        public List<string> Images { get; set; }

        public string Description { get; set; }

        public int Rooms { get; set; }

        public int Wc { get; set; }

        public int Parking { get; set; }

        public int SellerId { get; set; }
    }
}
