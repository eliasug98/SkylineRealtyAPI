using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkylineRealty.API.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int Phone { get; set; }

        public string Role { get; set; } = "Client";

        public DateTime CreatedDate { get; private set; } = DateTime.Now;

    }
}
