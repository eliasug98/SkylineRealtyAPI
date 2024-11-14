using AutoMapper;
using SkylineRealty.API.DTOs.UserDTOs;
using SkylineRealty.API.Entities;
using SkylineRealty.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace FurnitureStore.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _repository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UsersController(IUsersRepository repository, IMapper mapper, IConfiguration config)
        {
            _repository = repository;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate([FromBody] UserLoginDto credentials)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve todos los errores de validación
            }

            var user = _repository.ValidateCredentials(credentials);
            if (user == null)
            {
                return BadRequest("Incorrect email or password. Please try again");
            }


            var salt = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["AuthenticationConfiguration:Salt"])); //Traemos la SecretKey del Json. agregar antes: using Microsoft.IdentityModel.Tokens;
            var signingCredentials = new SigningCredentials(salt, SecurityAlgorithms.HmacSha256);

            //Los claims son datos en clave->valor que nos permite guardar data del usuario.
            var claims = new List<Claim>();
            claims.Add(new Claim("sub", user.Id.ToString())); //"sub" es una key estándar que significa unique user identifier, es decir, si mandamos el id del usuario por convención lo hacemos con la key "sub".
            claims.Add(new Claim("given_name", user.UserName)); //Lo mismo para given_name y family_name, son las convenciones para nombre y apellido. Ustedes pueden usar lo que quieran, pero si alguien que no conoce la app
            claims.Add(new Claim("Email", user.Email)); //quiere usar la API por lo general lo que espera es que se estén usando estas keys.
            claims.Add(new Claim("role", user.Role ?? "Client")); //Debería venir del usuario

            var jwtSecurityToken = new JwtSecurityToken( //add using System.IdentityModel.Tokens.Jwt; Acá es donde se crea el token con toda la data que le pasamos antes.
              _config["AuthenticationConfiguration:Issuer"],
              _config["AuthenticationConfiguration:Audience"],
              claims,
              DateTime.UtcNow,
              DateTime.UtcNow.AddHours(1),
              signingCredentials);

            string tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn); //return token(string)
        }

        [HttpGet("all")]
        [Authorize]
        public IActionResult GetUsers()
        {

            string role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Client";

            if (role != "Admin")
            {
                return Unauthorized("Not authorized to view users.");
            }

            List<User> users = _repository.GetUsers().ToList();

            var usersDto = _mapper.Map<List<UserDto>>(users);
            return Ok(usersDto);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? "0");
            var user = _repository.GetUser(userId); // Obtener el usuario por el ID del token

            if (user is null)
            {
                return NotFound("The user does not exist");
            }

            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetUser(int id)
        {
            string role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Client";
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? "0");

            if (role != "Admin")
            {
                if (userId != id)
                    return Unauthorized("Not authorized to view users.");
            }

            var user = _repository.GetUser(id);
            if (user is null)
            {
                return NotFound("The user does not exist");
            }

            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] UserToCreateDto userToCreate)
        {
            // Verifica si el modelo es válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve todos los errores de validación
            }

            if (_repository.UserNameExists(userToCreate.Username))
            {
                return BadRequest("Username already exist.");
            }

            if (_repository.EmailExists(userToCreate.Email))
            {
                return BadRequest("Email already exist");
            }

            // Hashear la contraseña usando la clase PasswordHasher
            var hashedPassword = _repository.HashPassword(userToCreate.Password);

            var user = _mapper.Map<User>(userToCreate);
            user.Password = hashedPassword; // Asigna la contraseña hasheada al usuario

            _repository.AddUser(user);
            _repository.SaveChanges();
            var userDto = _mapper.Map<UserDto>(user);

            return Created("Created", userDto);
        }

        [HttpPut("image")]
        [Authorize]
        public IActionResult UpdateProfilePicture([FromHeader] string image)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? "0");
            var user = _repository.GetUser(userId);

            if (user is null)
            {
                return NotFound("The user does not exist");
            }

            user.ProfilePicture = image;

            _repository.Update(user);

            _repository.SaveChanges();

            return Ok("Profile Picture updated succesfully");
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateUser([FromBody] UserToUpdateDto userUpdated)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? "0");
            var user = _repository.GetUser(userId); // Obtener el usuario por el ID del token
            if (user is null)
            {
                return NotFound("The user does not exist");
            }

            string role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Client";

            if (role != "Admin")
            {
                if (userId != user.Id)
                    return Unauthorized("Not authorized to update users.");
            }

            if (string.IsNullOrWhiteSpace(userUpdated.UserName))
            {
                user.UserName = user.UserName;  // Mantiene el nombre existente
            }
            else
            {
                user.UserName = userUpdated.UserName;  // Actualiza con el nuevo nombre
            }

            if (string.IsNullOrWhiteSpace(userUpdated.Email))
            {
                user.Email = user.Email;  // Mantiene el email existente
            }
            else
            {
                user.Email = userUpdated.Email;  // Actualiza con el nuevo email
            }

            _repository.Update(user);

            _repository.SaveChanges();

            return Ok("Name updated succesfully");
        }

        [HttpDelete("{idUser}")]
        [Authorize]
        public ActionResult DeleteUser(int idUser)
        {

            string role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Client";
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? "0");

            if (role != "Admin")
            {
                if (userId != idUser)
                    return Unauthorized("Not authorized to delete users.");
            }

            var userToDelete = _repository.GetUser(idUser);
            if (userToDelete is null)
                return NotFound();

            _repository.DeleteUser(userToDelete);
            _repository.SaveChanges();

            return NoContent();

        }
    }

}
