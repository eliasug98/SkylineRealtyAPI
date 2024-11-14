using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkylineRealty.API.DTOs.CommentDTOs;
using SkylineRealty.API.Entities;
using SkylineRealty.API.Services.Interfaces;
using System.Xml.Linq;

namespace SkylineRealty.API.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsRepository _repository;
        private readonly IMapper _mapper;

        public CommentsController(ICommentsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<Comment>> GetAllComments()
        {
            List<Comment>? comments = _repository.GetComments().ToList();
            if (comments.Count == 0)
                return NotFound("The property list is empty");

            return Ok(comments);
        }

        [HttpGet("{idProperty}", Name = "GetComments")]
        public IActionResult GetComments(int idProperty)
        {
            List<Comment>? comments = _repository.GetCommentsByPropertyId(idProperty).ToList();

            if (comments.Count == 0)
            {
                return NotFound("can´t find comments");
            }

            return Ok(comments);
        }

        [HttpPut("{idComment}")]
        public IActionResult ApproveComment(int idComment)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Client";

            if (role != "Admin")
            {
                return Unauthorized("Only admins can approve comments.");
            }

            var comment = _repository.GetCommentById(idComment);

            if (comment == null)
                return NotFound();

            comment.IsActive = !comment.IsActive;

            _repository.Update(comment);

            _repository.SaveChanges();

            return NoContent();
        }


        [HttpPost]
        [Authorize]
        public ActionResult<CommentDto> CreateComment([FromBody] CommentToCreateDto commentToCreate)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Client";
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? "0");

            if (role != "Client")
            {
                return Unauthorized("Only clients can post comments.");
            }

            if (commentToCreate == null)
                return NotFound();

            if (!_repository.CanUserComment(userId, commentToCreate.PropertyId))
            {
                return BadRequest("You can only post one comment per day for this property.");
            }

            var newComment = new Comment
            {
                Content = commentToCreate.Content,
                PropertyId = commentToCreate.PropertyId,
                UserId = userId
            };

            _repository.AddComment(newComment);

            var saved = _repository.SaveChanges();
            if (saved != true)
            {
                return BadRequest("comment could not be created");
            }

            return Created("Created", _mapper.Map<CommentDto>(newComment));
        }

        [HttpDelete("{idComment}")]
        [Authorize]
        public ActionResult DeleteComment(int idComment)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Client";

            if (role != "Admin")
            {
                return Unauthorized("Not authorized to delete comments.");
            }

            var commentToDelete = _repository.GetCommentById(idComment);
            if (commentToDelete == null)
                return NotFound();

            _repository.DeleteComment(commentToDelete);

            var saved = _repository.SaveChanges();
            if (saved != true)
            {
                return BadRequest("comment could not be deleted");
            }

            return NoContent();
        }
    }
}
