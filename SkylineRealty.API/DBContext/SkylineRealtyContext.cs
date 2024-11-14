using SkylineRealty.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SkylineRealty.API.DBContext
{
    public class SkylineRealtyContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Message> Messages { get; set; }

        public SkylineRealtyContext(DbContextOptions<SkylineRealtyContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la relación entre Property y User
            modelBuilder.Entity<Property>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuraciòn de la relación entre Comment y User
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User) // Cada comentario tiene un usuario
                .WithMany()
                .HasForeignKey(c => c.UserId) // Clave foránea en Comment
                .OnDelete(DeleteBehavior.Cascade); // Opcional: Eliminar comentarios si se elimina el usuario

            // Configuración de la relación entre Property y Comment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Property) // Un comentario pertenece a una propiedad
                .WithMany(p => p.Comments) // Una propiedad puede tener muchos comentarios
                .HasForeignKey(c => c.PropertyId) // Clave foránea en Comment
                .OnDelete(DeleteBehavior.Cascade); // Comportamiento al eliminar

            // Configuración de la relación entre Property y Image
            modelBuilder.Entity<Image>()
                .HasOne(i => i.Property) // Cada imagen pertenece a una propiedad
                .WithMany(p => p.Images) // Una propiedad puede tener muchas imágenes
                .HasForeignKey(i => i.PropertyId) // Clave foránea en Image
                .OnDelete(DeleteBehavior.Cascade); // Comportamiento al eliminar

            ////-----------------------------------------------------

            //var users = new User[3]
            //{
            //    new User()
            //    {
            //        "id": 1,
            //        "name": Jane,
            //        "Lastname": Smith,
            //        "username": "user1",
            //        "email": "user1@example.com",
            //        "password": "asdfgh",
            //        "phone": 666
            //        "role": "Admin"
            //    },
            //    new User()
            //    {
            //        "id": 2,
            //        "name": John,
            //        "Lastname": Doe,
            //        "username": "user2",
            //        "email": "user2@example.com",
            //        "password": "qwerty",
            //        "phone": 123
            //        "role": "Client"
            //    },
            //};
            //modelBuilder.Entity<User>().HasData(users);

            base.OnModelCreating(modelBuilder);
        }
    }
}