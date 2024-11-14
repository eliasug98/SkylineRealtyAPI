using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace SkylineRealty.API.Entities
{
    public class Property
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public ICollection<Image> Images { get; set; } = new List<Image>();

        [Required]
        public string Description { get; set; }

        [Required]
        public int Beds { get; set; }

        [Required]
        public int Baths { get; set; }

        [Required]
        public int Garages { get; set; }

        [Required]
        public int Sqft { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public int SellerId { get; set; }

        [ForeignKey("SellerId")]
        public User User { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>(); 
    }
}
