using AutoMapper;
using BLL.Models;
using DAL;
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
        public IncidentController(ApplicationDbContext applicationDbContext,
            IMapper mapper)
        {
            _dbContext = applicationDbContext;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] IncidentModel incident)
        {
            var accounts = _mapper.Map<ICollection<AccountModel>, ICollection<Account>>(incident.Accounts);
            var accountWithExistingNames = accounts.Where(i => _dbContext.Accounts.Select(i => i.Name).Contains(i.Name)).ToList();
            var newIncident = _mapper.Map<IncidentModel, Incident>(incident);
            try
            {
                if (accountWithExistingNames.Count > 0) {
                    
                    foreach (var account in accountWithExistingNames)
                    {
                        accounts.Remove(account);
                    }
                    newIncident.Accounts = accounts;
                    var result = await _dbContext.Incidents.AddAsync(newIncident);
                    await _dbContext.SaveChangesAsync();
                    foreach(var account in accountWithExistingNames)
                    {
                        var sqlQuery = $"Update Accounts Set IncidentName = '{result.Entity.Name}' Where Name = '{ account.Name}'";
                        _dbContext.Database.ExecuteSqlRaw(sqlQuery);
                    }
                    await _dbContext.SaveChangesAsync();
                    return Ok(result.Entity);
                }
                newIncident.Accounts = accounts;
                var result1 = await _dbContext.Incidents.AddAsync(newIncident);
                await _dbContext.SaveChangesAsync();
                return Ok(result1.Entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
