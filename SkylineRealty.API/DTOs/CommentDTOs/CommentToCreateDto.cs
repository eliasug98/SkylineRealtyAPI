namespace SkylineRealty.API.DTOs.CommentDTOs
{
    public class CommentToCreateDto
    {
        public string Content { get; set; }
        public int PropertyId { get; set; }
        public int UserId { get; set; }
    }
}
