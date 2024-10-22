using System.ComponentModel.DataAnnotations;

namespace SkylineRealty.API.DTOs.UserDTOs
{
    public class UserToCreateDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required]
        public int Phone { get; set; }

    }
}
