using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalEnvision.Data;
using DigitalEnvision.Models;
using Microsoft.EntityFrameworkCore;
using UT.Interface;

namespace UT.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UTContext _context;
        public UserRepository(UTContext context)
        {
            this._context = context;
        }

        public void Insert<T>(T entity)
        {
            _context.AddAsync(entity);
        }

        public void Update<T>(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete<T>(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUserByIdAsync(int Id)
        {
            return await _context.User.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await _context.User.ToListAsync();
        }
    }
}
