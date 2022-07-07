using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalEnvision.Models;

namespace UT.Interface
{
    public interface IUserRepository
    {
        Task<bool> SaveAllAsync();
        void Insert<T>(T entity);
        void Update<T>(T entity);
        void Delete<T>(T entity);

        Task<User> GetUserByIdAsync(int Id);
        Task<IEnumerable<User>> GetAllUserAsync();
    }
}
