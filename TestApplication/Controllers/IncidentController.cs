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
            var existingAccountsNames = _dbContext.Accounts.Select(i=>i.Name);
            var ifAreAccountsWithExistingNames = accounts.Select(i=>i.Name).Intersect(existingAccountsNames).Any();
            if (ifAreAccountsWithExistingNames) {

                return BadRequest("One or more accounts already exists");
            }
            var newIncident = _mapper.Map<IncidentModel, Incident>(incident);
            newIncident.Accounts = accounts;
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
