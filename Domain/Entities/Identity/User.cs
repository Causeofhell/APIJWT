﻿namespace Domain.Entities.Identity
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public RoleEntity Role { get; set; } // User has one RoleEntity

        public int RoleEntityId { get; set; } // Foreign key for RoleEntity

        public User()
        {
            Role = new RoleEntity();
        }
    }
}
