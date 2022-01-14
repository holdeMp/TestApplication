using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ContactRepository:IContactRepository
    {
        private readonly ApplicationDbContext _db;
        public ContactRepository(ApplicationDbContext forumDbContext)
        {
            _db = forumDbContext;
        }
        public async Task AddAsync(Contact entity)
        {
            await _db.Contacts.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async void Update(Contact entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
    }
}
