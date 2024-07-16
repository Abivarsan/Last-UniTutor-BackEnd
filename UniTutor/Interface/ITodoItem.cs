using UniTutor.Model;

namespace UniTutor.Interface
{
    public interface ITodoItem
    {
        Task<TodoItem> GetByIdAsync(int id);
        Task<List<TodoItem>> GetAllAsync();
        Task<List<TodoItem>> GetByStudentIdAsync(int studentId);
        Task<List<TodoItem>> GetByTutorIdAsync(int tutorId);
        Task<TodoItem> CreateAsync(TodoItem todoItem);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(TodoItem todoItem);


    }
}

