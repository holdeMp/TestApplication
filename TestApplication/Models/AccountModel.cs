using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class AccountModel
    {
        public string Name { get; set; }
      
        public ICollection<ContactModel> Contacts { get; set; }
    }
}
