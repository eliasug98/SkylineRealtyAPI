namespace SkylineRealty.API.DTOs.UserDTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Phone { get; set; }
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
