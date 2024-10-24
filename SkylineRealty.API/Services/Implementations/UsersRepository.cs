using SkylineRealty.API.DBContext;
using SkylineRealty.API.DTOs.UserDTOs;
using SkylineRealty.API.Entities;
using SkylineRealty.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace SkylineRealty.API.Services.Implementations
{
    public class UsersRepository : IUsersRepository
    {
        private readonly SkylineRealtyContext _context;
        public UsersRepository(SkylineRealtyContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users;
        }


        public User? GetUser(int idUser)
        {
            return _context.Users.FirstOrDefault(u => u.Id == idUser);
        }

        public void AddUser(User newUser)
        {
            _context.Users.Add(newUser);
        }

        public void Update(User user)
        {
            _context.Update(user);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public bool EmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool UserNameExists(string name)
        {
            return _context.Users.Any(u => u.UserName == name);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public User? ValidateCredentials(UserLoginDto authParams)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == authParams.Email);

            if (user != null && VerifyPassword(authParams.Password, user.Password))
            {
                return user;
            }

            return null;
        }

        public string HashPassword(string password)
        {
            // Crear un nuevo salt
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16]; // 16 bytes es suficiente
                rng.GetBytes(salt);

                // Hashing utilizando PBKDF2
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000)) // 100,000 iteraciones
                {
                    byte[] hash = pbkdf2.GetBytes(32); // Generar un hash de 32 bytes (256 bits)

                    // Combinar el salt y el hash para almacenarlo
                    byte[] hashWithSalt = new byte[salt.Length + hash.Length];
                    Buffer.BlockCopy(salt, 0, hashWithSalt, 0, salt.Length);
                    Buffer.BlockCopy(hash, 0, hashWithSalt, salt.Length, hash.Length);

                    return Convert.ToBase64String(hashWithSalt); // Devolver como string
                }
            }
        }

        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Convertir el hash almacenado de vuelta a bytes
            byte[] hashWithSalt = Convert.FromBase64String(storedHash);

            // Extraer el salt del hash almacenado
            byte[] salt = new byte[16];
            Buffer.BlockCopy(hashWithSalt, 0, salt, 0, salt.Length);

            // Hashear la contraseña ingresada con el mismo salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 100000))
            {
                byte[] enteredHash = pbkdf2.GetBytes(32); // Generar un hash de 32 bytes (256 bits)

                // Comparar los hashes
                for (int i = 0; i < enteredHash.Length; i++)
                {
                    if (hashWithSalt[i + salt.Length] != enteredHash[i])
                        return false;
                }
                return true; // Las contraseñas coinciden
            }
        }

    }
}
