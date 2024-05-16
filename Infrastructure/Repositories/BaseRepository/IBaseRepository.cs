using System.Linq.Expressions;

namespace Infrastructure.Repositories.BaseRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task RemoveAsync(TEntity item);
        Task UpdateAsync(TEntity entity);
        Task<TEntity> GetAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllWhereAsync(Expression<Func<TEntity, bool>> filter);
    }
}
