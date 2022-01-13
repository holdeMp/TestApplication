
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public virtual Account Account { get; set; }
    }
}
