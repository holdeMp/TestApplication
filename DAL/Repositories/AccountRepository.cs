using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;
        public AccountRepository(ApplicationDbContext forumDbContext)
        {
            _db = forumDbContext;
        }
        public Task<Account> FindByNameAsync(string accountName)
        {
            return Task.Run(() => { return _db.Accounts.FirstOrDefault(i => i.Name == accountName); }); ;
        }
    }
}
