using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caminhao.Api.Domain.UserAggregate
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public Sex Sex { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public override bool Equals(object obj) => obj is User user &&
                   Id == user.Id &&
                   Firstname == user.Firstname &&
                   Lastname == user.Lastname &&
                   Middlename == user.Middlename &&
                   Sex == user.Sex &&
                   Email == user.Email &&
                   Username == user.Username &&
                   Password == user.Password;

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Firstname);
            hash.Add(Lastname);
            hash.Add(Middlename);
            hash.Add(Sex);
            hash.Add(Email);
            hash.Add(Username);
            hash.Add(Password);
            return hash.ToHashCode();
        }
    }
}
