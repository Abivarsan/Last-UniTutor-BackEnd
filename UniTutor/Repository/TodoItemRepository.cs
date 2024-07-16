using Microsoft.EntityFrameworkCore;
using UniTutor.DataBase;
using UniTutor.Interface;
using UniTutor.Model;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace UniTutor.Repository
{
    public class TodoItemRepository : ITodoItem
    {
        private ApplicationDBContext _DBcontext;
        private readonly IConfiguration _config;
        public TodoItemRepository(ApplicationDBContext DBcontext, IConfiguration config)
        {
            _DBcontext = DBcontext;
            _config = config;
        }
        public async Task<TodoItem> GetByIdAsync(int id)
        {
            return await _DBcontext.TodoItems.FindAsync(id);
        }

        public async Task<List<TodoItem>> GetAllAsync()
        {
            return await _DBcontext.TodoItems.ToListAsync();
        }

        public async Task<List<TodoItem>> GetByStudentIdAsync(int studentId)
        {
            return await _DBcontext.TodoItems
                .Where(t => t.studentId == studentId)
                .ToListAsync();
        }

        public async Task<List<TodoItem>> GetByTutorIdAsync(int tutorId)
        {
            return await _DBcontext.TodoItems
                .Where(t => t.tutorId == tutorId)
                .ToListAsync();
        }

        public async Task<TodoItem> CreateAsync(TodoItem todoItem)
        {
            try
            {
                _DBcontext.TodoItems.Add(todoItem);
                await _DBcontext.SaveChangesAsync();
                return todoItem;
            }
            catch (Exception ex)
            {
                // Log the inner exception for more details
                var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                // Example: Log to console or a logging framework
                Console.WriteLine($"Inner Exception Message: {innerExceptionMessage}");
                // Rethrow the exception or handle as needed
                throw new Exception("An error occurred while saving the TodoItem.", ex);
            }
        }
        //public async Task CreateAsync(string text, int Id, string usertype)
        //{


        //    var todolist = new TodoItem
        //    {
        //        text = text,

        //    };

        //    // Set the appropriate ID based on the usertype
        //    if (usertype.Equals("Student", StringComparison.OrdinalIgnoreCase))
        //    {
        //        todolist.studentId = Id;
        //    }
        //    else if (usertype.Equals("Tutor", StringComparison.OrdinalIgnoreCase))
        //    {
        //        todolist.tutorId = Id;
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Invalid user type. Must be 'Student' or 'Tutor'.");
        //    }

        //    await AddTodoAsync(todolist);
        //}
        //public async Task AddTodoAsync(TodoItem todolist)
        //{
        //    _DBcontext.TodoItems.Add(todolist);
        //    await _DBcontext.SaveChangesAsync();
        //}


        public async Task<bool> DeleteAsync(int id)
        {
            var todoItem = await _DBcontext.TodoItems.FindAsync(id);
            if (todoItem == null)
                return false;

            _DBcontext.TodoItems.Remove(todoItem);
            await _DBcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(TodoItem todoItem)
        {
            _DBcontext.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _DBcontext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(todoItem._id))
                    return false;
                else
                    throw;
            }
        }

        private bool TodoItemExists(int id)
        {
            return _DBcontext.TodoItems.Any(e => e._id == id);
        }
    }
}