public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(string id); // Change from int to string
    Task<T> GetByEmailAsync(string email);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id); // Change from int to string
    Task SaveChangesAsync();
}