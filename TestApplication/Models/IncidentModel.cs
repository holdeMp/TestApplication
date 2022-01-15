using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class IncidentModel
    {
        public string Description {get;set;}
        public ICollection<IncidentAccountModel> Accounts { get; set; }
    }
}
