using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DAL
{
    [Index(nameof(Name), IsUnique = true)]
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Incident Incident { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}