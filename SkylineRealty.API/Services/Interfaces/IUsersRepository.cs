using SkylineRealty.API.DTOs.UserDTOs;
using SkylineRealty.API.Entities;

namespace SkylineRealty.API.Services.Interfaces
{
    public interface IUsersRepository
    {
        void AddUser(User newUser);
        void DeleteUser(User user);
        bool EmailExists(string email);
        User? GetUser(int idUser);
        IEnumerable<User> GetUsers();
        string HashPassword(string password);
        bool SaveChanges();
        void Update(User user);
        bool UserNameExists(string name);
        User? ValidateCredentials(UserLoginDto authParams);
        bool VerifyPassword(string enteredPassword, string storedHash);
    }
}
