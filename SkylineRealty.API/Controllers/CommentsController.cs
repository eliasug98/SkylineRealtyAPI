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
    [Route("api/[controller]")]
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

        [HttpGet("{idProperty}", Name = "GetComments")]
        public IActionResult GetComments(int idProperty)
        {
            List<Comment>? comments = _repository.GetCommentsByPropertyId(idProperty).ToList();

            if (comments.Count == 0)
            {
                return NotFound("can´t find comments");
            }

            var commentsDto = _mapper.Map<List<CommentDto>>(comments);

            return Ok(commentsDto);
        }


        [HttpPost]
        [Authorize]
        public ActionResult<CommentDto> CreateComment([FromBody] CommentToCreateDto commentToCreate)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Client";

            if (commentToCreate == null)
                return NotFound();

            var newComment = _mapper.Map<Comment>(commentToCreate);

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
