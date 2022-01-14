using AutoMapper;
using BLL.Models;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public AccountController(ApplicationDbContext applicationDbContext,
            IMapper mapper)
        {
            _dbContext = applicationDbContext;
            _mapper = mapper; 
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] AccountModel account)
        {
            var contactsEmails = _dbContext.Contacts.Select(i => i.Email);
            var ifAreContactsWithExistingEmail = account.Contacts.Select(i=>i.Email).Intersect(contactsEmails).Any();

            if (ifAreContactsWithExistingEmail)
            {
                return BadRequest("One or more contacts already exists");
            }
            var newAccount = _mapper.Map<AccountModel, Account>(account);
            try
            {
                
                var result = await _dbContext.Accounts.AddAsync(newAccount);
                await _dbContext.SaveChangesAsync();
                return Ok(result.Entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
