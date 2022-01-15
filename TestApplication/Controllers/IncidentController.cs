using AutoMapper;
using BLL.Models;
using DAL;
using DAL.Interfaces;
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
    public class IncidentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        public IncidentController(ApplicationDbContext applicationDbContext,
            IMapper mapper,
            IAccountRepository accountRepository)
        {
            _dbContext = applicationDbContext;
            _mapper = mapper;
            _accountRepository = accountRepository;
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] IncidentModel incident)
        {
            var newIncident = new Incident { Description = incident.Description , Accounts = new List<Account>() };

            foreach (var accountName in incident.Accounts)
            {
                var account = _accountRepository.FindByNameAsync(accountName.Name).Result;
                if (account != null)
                {
                    newIncident.Accounts.Add(account);
                    account.Incident = newIncident;
                    
                    continue;
                }
                var newAccount = new Account { Incident = newIncident, Name = accountName.Name };
                newIncident.Accounts.Add(newAccount);
            }
            try
            {
                var result = await _dbContext.Incidents.AddAsync(newIncident);
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
