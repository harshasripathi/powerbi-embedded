namespace powerbi_embedded_api.DA.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAsync();
        Task<T?> GetByIdAsync(long id);
        IEnumerable<T>? GetList(string key, object value);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveAsync();
    }
}