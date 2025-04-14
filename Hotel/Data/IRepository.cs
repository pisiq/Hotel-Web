using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotel.Data
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetByEmailAsync(string email);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}