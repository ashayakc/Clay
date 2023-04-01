using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class LockDbContext : DbContext
    {
        public LockDbContext(DbContextOptions<LockDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Door>(entity =>
            {
                entity.ToTable("Doors");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.Doors)
                    .HasForeignKey(d => d.OfficeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Doors_OfficeId");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasIndex(x => x.UserName)
                    .HasDatabaseName("IX_User_UserName")
                    .IsUnique();
            });

            modelBuilder.Entity<RoleDoorMapping>(entity =>
            {
                entity.ToTable("RoleDoorMappings");

                entity.HasOne(d => d.Door)
                    .WithMany(p => p.RoleDoorMappings)
                    .HasForeignKey(d => d.DoorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleDoorMapping_DoorId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleDoorMappings)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleDoorMapping_RoleId");

                entity.HasIndex(x => new { x.RoleId, x.DoorId })
                    .HasDatabaseName("IX_RoleDoorMapping_RoleId_DoorId");
            });

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, RoleId = 1, Firstname = "Sheldon", LastName = "Cooper", EmailId = "sheldoncooper@gmail.com", IsAdmin = 1, EmployeeId = "1000", UserName = "sheldon", Password = "c2hlbGRvbg==" },
                new User { Id = 2, RoleId = 2, Firstname = "Adia", LastName = "Bugg", EmailId = "adia@gmail.com", IsAdmin = 0, EmployeeId = "1001", UserName = "adia", Password = "YWRpYQ==" },
                new User { Id = 3, RoleId = 3, Firstname = "Olive", LastName = "yew", EmailId = "olive@gmail.com", IsAdmin = 0, EmployeeId = "1002", UserName = "olive", Password = "b2xpdmU=" },
                new User { Id = 4, RoleId = 3, Firstname = "Peg", LastName = "Legge", EmailId = "peg@gmail.com", IsAdmin = 0, EmployeeId = "1003", UserName = "peg", Password = "cGVn" },
                new User { Id = 5, RoleId = 4, Firstname = "Allie", LastName = "Grater", EmailId = "allie@gmail.com", IsAdmin = 0, EmployeeId = "1004", UserName = "allie", Password = "YWxsaWU=" }
            );
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Manager", Description = "" },
                new Role { Id = 2, Name = "Director", Description = "" },
                new Role { Id = 3, Name = "Developer", Description = "" },
                new Role { Id = 4, Name = "Finance", Description = "" }
                );

            modelBuilder.Entity<Office>().HasData(
                new Office { Id = 1, Name = "Clay", Address = "Amsterdam" },
                new Office { Id = 2, Name = "Philips", Address = "Eindhoven" }
                );

            modelBuilder.Entity<Door>().HasData(
                new Door { Id = 1, Name = "FrontDoor", Type = "Main", OfficeId = 1 },
                new Door { Id = 2, Name = "BackDoor", Type = "Back", OfficeId = 1 },
                new Door { Id = 3, Name = "StoreRoom", Type = "Store", OfficeId = 1 },
                new Door { Id = 4, Name = "FrontDoor", Type = "Main", OfficeId = 2 }
                );

            modelBuilder.Entity<RoleDoorMapping>().HasData(
                new RoleDoorMapping { Id = 1, RoleId = 1, DoorId = 1 },
                new RoleDoorMapping { Id = 2, RoleId = 2, DoorId = 1 },
                new RoleDoorMapping { Id = 3, RoleId = 3, DoorId = 1 },
                new RoleDoorMapping { Id = 4, RoleId = 4, DoorId = 1 },
                new RoleDoorMapping { Id = 5, RoleId = 1, DoorId = 2 },
                new RoleDoorMapping { Id = 6, RoleId = 2, DoorId = 2 },
                new RoleDoorMapping { Id = 7, RoleId = 3, DoorId = 3 },
                new RoleDoorMapping { Id = 8, RoleId = 4, DoorId = 4 }
                );
        }

        public DbSet<User> Users { get; }
        public DbSet<Door> Doors { get; }
        public DbSet<Role> Roles { get; }
        public DbSet<Office> Offices { get; }
        public DbSet<RoleDoorMapping> UserDoorMappings { get; }
    }
}
