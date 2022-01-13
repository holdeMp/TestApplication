using BLL;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public ContactController(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;

        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] AddContactModel addContactModel)
        {
            var account = _dbContext.Accounts.FirstOrDefault(i => addContactModel.AccountName == i.Name);
            if (account == null)
            {
                return NotFound();
            }
            Incident incident = new Incident { Description = addContactModel.IncidentDescription };
            account.Incident = incident;
            var existingContact = _dbContext.Contacts.FirstOrDefault(i => i.Email == addContactModel.ContactEmail);
            if (existingContact != null)
            {
                existingContact.Email = addContactModel.ContactEmail;
                existingContact.FirstName = addContactModel.ContactFirstName;
                existingContact.LastName = addContactModel.ContactLastName;
                existingContact.Account = account;
                try
                {
                    _dbContext.Entry(existingContact).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                    return Ok(existingContact);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            Contact contact = new Contact
            {
                Email = addContactModel.ContactEmail,
                FirstName = addContactModel.ContactFirstName,
                LastName = addContactModel.ContactLastName,
                Account = account
            };
            try
            {
                var result = await _dbContext.Contacts.AddAsync(contact);
                return Ok(result.Entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
