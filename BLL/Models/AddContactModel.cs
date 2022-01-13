using System;

namespace BLL
{
    public class AddContactModel
    {
        public string AccountName { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactEmail { get; set; } // unique identifier,
        public string IncidentDescription { get; set; }
    }
}
