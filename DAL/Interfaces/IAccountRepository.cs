using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAccountRepository
    {
        public Task<Account> FindByNameAsync(string accountName);
    }
}
