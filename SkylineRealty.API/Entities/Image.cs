using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkylineRealty.API.Entities
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public int PropertyId { get; set; }

        [JsonIgnore]
        [ForeignKey("PropertyId")]
        public Property Property { get; set; }
    }
}
