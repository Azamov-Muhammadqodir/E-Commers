using E_Commers.Domain.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commers.Domain.Entity
{
    public class User:BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone {  get; set; }
       // public string PasswordHash {  get; set; }
        public string PasswordSold {  get; set; }
        // [JsonPropertyName("UserRoleNames")]
        [JsonIgnore]
        public ICollection<UserRole>? UserRoles { get; set; }
        [JsonPropertyName("Roles")]
        [NotMapped]
        public Guid[] _roles {  get; set; }
    }
}
