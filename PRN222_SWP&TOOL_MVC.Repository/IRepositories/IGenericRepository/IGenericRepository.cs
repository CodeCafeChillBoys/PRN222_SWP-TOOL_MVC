using System.Linq.Expressions;

namespace PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        // Lấy toàn bộ danh sách (Thường dùng Task<IEnumerable<T>>)
        Task<IEnumerable<T>> GetAllAsync();

        // Dùng ValueTask cho GetById vì nếu dữ liệu đã có trong memory, nó sẽ nhanh hơn
        ValueTask<T?> GetByIdAsync(object id);

        // Thêm mới
        Task AddAsync(T entity);

        // Cập nhật (EF Core thường không có UpdateAsync vì nó theo dõi trạng thái, 
        // nhưng Task vẫn giúp đồng bộ hóa quy trình)
        Task UpdateAsync(T entity);

        // Xóa
        Task DeleteAsync(object id);

        // Kiểm tra tồn tại với biểu thức điều kiện
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        // Đếm số lượng
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);

        public Task<T?> FindWitInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
}
