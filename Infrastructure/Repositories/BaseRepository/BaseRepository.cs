using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Infrastructure.Repositories.BaseRepository
{

    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {

        private readonly DbSet<TEntity> _entity;

        private readonly ApplicationContext _context;
        public readonly string _userId;


        public BaseRepository(ApplicationContext entity)
        {
            _entity = entity.Set<TEntity>();
            _context = entity;
            _userId = _context._contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        }

        public async Task AddAsync(TEntity entity)
        {
            await _entity.AddAsync(entity);

            _context.SaveChanges();
        }

        public async Task RemoveAsync(TEntity item)
        {
            _context.Remove(item);

            _context.SaveChanges();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _entity.Update(entity);

            _context.SaveChanges();
        }

        public async Task<TEntity> GetAsync(int id)
        {
            var result = await _entity.FindAsync(id);

            return result;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var result = await _entity.AsNoTracking().ToListAsync();

            return result;
        }

        public async Task<IEnumerable<TEntity>> GetAllWhereAsync(Expression<Func<TEntity,bool>> filter)
        {
            var result = _entity.Where(filter) ;

            return result;
        }
    }
}
