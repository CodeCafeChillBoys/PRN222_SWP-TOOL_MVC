using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace PRN222_SWP_TOOL_MVC.Repository.Repositories.GenericRepository
{
    public class GenericRepository<T> : IRepositories.IGenericRepository.IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        public Task DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async ValueTask<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        {
            // FirstOrDefaultAsync sẽ trả về đối tượng đầu tiên thỏa mãn điều kiện
            // Nếu không tìm thấy, nó sẽ trả về null
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<T?> FindWitInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(predicate);
        }


        public async Task<IEnumerable<T>> GetAllWithIncludeAsync(
    params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

    }
}
