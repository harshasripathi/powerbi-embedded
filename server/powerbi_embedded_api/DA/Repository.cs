using Microsoft.EntityFrameworkCore;
using powerbi_embedded_api.DA.Interfaces;
using powerbi_embedded_api.Exceptions;
using System;

namespace powerbi_embedded_api.DA
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<T> _entities;
        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _entities = _dataContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(long id)
        {
            return await _entities.FindAsync(id);
        }

        public IEnumerable<T>? GetList(string key, object value)
        {
            var property = typeof(T).GetProperties().Where(i => i.Name == key).SingleOrDefault();
            Type? type = property?.PropertyType;

            if (value.GetType() == type)
            {
                return _entities!.AsEnumerable().Where(i => i!.GetType()!.GetProperty(property!.Name)!.GetValue(i)!.ToString() == value.ToString());
            }

            else
                throw new ApiException("Invalid type");
        }

        public async Task InsertAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            await _entities.AddAsync(entity);
            await SaveAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            _entities.Update(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            _entities.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
