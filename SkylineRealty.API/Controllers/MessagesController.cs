using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkylineRealty.API.DTOs.MessageDTOs;
using SkylineRealty.API.Entities;
using SkylineRealty.API.Services.Interfaces;

namespace SkylineRealty.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesRepository _repository;

        public MessagesController(IMessagesRepository repository)
        {
            _repository = repository;
        }

        // GET: api/messages
        [HttpGet]
        public ActionResult<List<Message>> GetMessages()
        {
            var messages = _repository.GetMessages().ToList();
            return Ok(messages);
        }

        // GET: api/messages/user/{userId}
        [HttpGet("user/{userId}")]
        [Authorize]
        public ActionResult<List<Message>> GetUserMessages(int userId)
        {
            var currentUserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);
            if (currentUserId != userId)
            {
                return Unauthorized("Not authorized to view these messages");
            }

            var messages = _repository.GetMessagesByUserId(userId).ToList();

            return Ok(messages);
        }

        // POST: api/messages
        [HttpPost]
        [Authorize]
        public ActionResult<Message> PostMessage([FromBody] CreateMessageDto createMessageDto)
        {
            if (string.IsNullOrWhiteSpace(createMessageDto.Content))
            {
                return BadRequest("The message content cannot be empty.");
            }

            string role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Client";
            Message message = new Message();

            if (role == "Admin")
            {
                var adminId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);
                message.AdminId = adminId;

                if (createMessageDto.UserId.HasValue)
                {
                    message.UserId = createMessageDto.UserId.Value;
                }
                else
                {
                    return BadRequest("The user ID must be provided for administrator messages.");
                }

                message.Content = createMessageDto.Content;
            }
            else
            {
                message.UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? "0");
                message.Content = createMessageDto.Content;
                message.AdminId = 0;
            }

            _repository.AddMessage(message);
            _repository.SaveChanges();

            return Created("Created", message);
        }

        // POST: api/messages/markAsRead/{userId}
        [HttpPost("markAsRead/{userId}")]
        [Authorize]
        public IActionResult MarkMessagesAsRead(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("User ID is required.");
            }

            var currentUserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);
            List<Message> unreadMessages;

            if (currentUserId == userId)
            {
                unreadMessages = _repository.GetUnreadAdminMessages(userId).ToList();
            }
            else
            {
                unreadMessages = _repository.GetUnreadUserMessages(userId).ToList();
            }

            if (!unreadMessages.Any())
            {
                return NotFound("No unread messages found.");
            }

            foreach (var message in unreadMessages)
            {
                message.IsRead = true;

            }

            _repository.SaveChanges();

            return Ok();
        }
    }
}
