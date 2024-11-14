using AutoMapper;
using SkylineRealty.API.Entities;
using SkylineRealty.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SkylineRealty.API.DTOs.PropertyDTOs;
using SkylineRealty.API.DTOs.UserDTOs;

namespace SkylineRealty.API.Controllers
{
    [ApiController]
    [Route("api/properties")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertiesRepository _repository;
        private readonly IMapper _mapper;

        public PropertiesController(IPropertiesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<Property>> GetProperties()
        {
            List<Property>? properties = _repository.GetProperties().ToList();
            if (properties.Count == 0)
                return NotFound("The property list is empty");

            return Ok(properties);
        }

        [HttpGet("{idProperty}", Name = "GetProperty")]
        public IActionResult GetProperty(int idProperty)
        {
            Entities.Property? property = _repository.GetPropertyById(idProperty);

            if (property == null)
            {
                return NotFound("can´t find property");
            }

            return Ok(property);
        }

        [HttpGet("user")]
        public ActionResult<List<Property>> GetUserProperties()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? "0");

            List<Property>? properties = _repository.GetPropertiesBySellerId(userId).ToList();

            if (properties.Count == 0)
                return NotFound("The property list is empty");

            return Ok(properties);
        }


        [HttpPost]
        [Authorize]
        public ActionResult<Property> CreateProperty([FromBody] PropertyToCreateDto propertyToCreate)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Client";

            if (role != "Admin")
            {
                return Unauthorized("Not authorized to create properties.");
            }

            if (propertyToCreate == null)
                return NotFound();

            if (_repository.PropertyExists(propertyToCreate.Title))
                return BadRequest();

            var newProperty = _mapper.Map<Property>(propertyToCreate);

            _repository.AddProperty(newProperty);

            var saved = _repository.SaveChanges();
            if (saved != true)
            {
                return BadRequest("property could not be created");
            }

            return Created("Created", newProperty);
        }

        [HttpPut("{idProperty}")]
        [Authorize]
        public ActionResult UpdateProperty(int idProperty, [FromBody] PropertyToUpdateDto propertyUpdated)
        {

            string role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Client";

            if (role != "Admin")
            {
                return Unauthorized("Not authorized to update properties.");
            }

            var propertyToUpdate = _repository.GetPropertyById(idProperty);
            if (propertyToUpdate == null)
                return NotFound();

            _mapper.Map(propertyUpdated, propertyToUpdate);

            _repository.Update(propertyToUpdate);

            var saved = _repository.SaveChanges();
            if (saved != true)
            {
                return BadRequest("property could not be updated");
            }

            return NoContent();
        }

        [HttpDelete("{idProperty}")]
        [Authorize]
        public ActionResult DeleteProperty(int idProperty)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Client";
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? "0");

            var propertyToDelete = _repository.GetPropertyById(idProperty);
            if (propertyToDelete == null)
                return NotFound();

            if (role != "Admin")
            {
                if (userId != propertyToDelete.SellerId)
                    return Unauthorized("Not authorized to delete properties.");
            }

            _repository.DeleteProperty(propertyToDelete);

            var saved = _repository.SaveChanges();
            if (saved != true)
            {
                return BadRequest("property could not be deleted");
            }

            return NoContent();
        }

    }
}