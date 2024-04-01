
namespace Domain.Entities.Identity
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string Role { get; set; } = "user";
        public User User { get; set; } // RoleEntity belongs to one User
    }
}
