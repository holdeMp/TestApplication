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
            var contactsWithExistingEmail = account.Contacts.Where(i=> _dbContext.Contacts.Select(i=>i.Email).Contains(i.Email)).ToList();
            var contactsWithExistingEmailWithIds = _dbContext.Contacts.Where(i => account.Contacts.Select(i => i.Email).Contains(i.Email)).ToList();
            try
            {
                if (contactsWithExistingEmail.Count > 0)
                {
                    foreach (var contact in contactsWithExistingEmail) {
                        account.Contacts.Remove(contact);
                    }
                    
                    await _dbContext.Accounts.AddAsync(_mapper.Map<AccountModel, Account>(account));
                    await _dbContext.SaveChangesAsync();
                    var accountId = _dbContext.Accounts.OrderByDescending(p => p.Id).FirstOrDefault().Id;
                    foreach (var contact in contactsWithExistingEmailWithIds)
                    {
                        _dbContext.Database.ExecuteSqlRaw($"Update Contacts Set AccountId = {accountId} Where Id = {contact.Id}");
                    }
                    await _dbContext.SaveChangesAsync();
                    return Ok(_dbContext.Accounts.OrderByDescending(p => p.Id).FirstOrDefault());
                }
                var newAccount = _mapper.Map<AccountModel, Account>(account);
                await _dbContext.Accounts.AddAsync(newAccount);
                await _dbContext.SaveChangesAsync();
                return Ok(_dbContext.Accounts.OrderByDescending(p => p.Id).FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
