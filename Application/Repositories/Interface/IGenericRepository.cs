using Domain.Entities;

namespace Application.Repositories.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int Id);
        IQueryable<T> GetAll();
        Task Insert(T entity);
        Task InsertRange(List<T> entity);
        void Update(T entity);
        Task Delete(T entity);
        Task DeleteRange(List<T> entity);
        Task<int> SaveChanges();

    }
}
