namespace SkylineRealty.API.DTOs.CommentDTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PropertyId { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
