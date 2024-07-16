using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniTutor.Interface;
using UniTutor.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniTutor.DTO;
using Microsoft.EntityFrameworkCore;
using UniTutor.Repository;
using System.Xml.Linq;

namespace UniTutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly ITodoItem _todoItem;
        private readonly IEmailService _emailService;

        public TodoItemController(ITodoItem todoItem, IEmailService emailService)
        {
            _todoItem = todoItem;
            _emailService = emailService;
        }

        // GET: api/Todos/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodosByStudentId(int studentId)
        {
            var todos = await _todoItem.GetByStudentIdAsync(studentId);
            
            return Ok(todos);
        }

        //get all todos by tutorId
        [HttpGet("tutor/{tutorId}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodosByTutorId(int tutorId)
        {
            var todos = await _todoItem.GetByTutorIdAsync(tutorId);
            return Ok(todos);
        }

        [HttpPost("{usertype}/{id}")]
        public async Task<ActionResult<TodoItem>> PostTodo(string usertype, int id, TodoItemDto todoItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var todoItem = new TodoItem
                {
                    text = todoItemDto.text,
                    isCompleted = false
                };

                if (usertype == "Student")
                {
                    todoItem.studentId = id;
                }
                else if (usertype == "Tutor")
                {
                    todoItem.tutorId = id;
                }
                else
                {
                    return BadRequest("Invalid usertype. Expected 'student' or 'tutor'.");
                }

                var newTodo = await _todoItem.CreateAsync(todoItem);
                return CreatedAtAction(nameof(GetTodos), new { id = newTodo._id }, newTodo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }
        //[HttpPost("create/{Id}/{usertype}")]
        //public async Task<IActionResult> CreateStudentComment(int Id, string usertype, [FromBody] TodoDto todoDto)
        //{


        //    await _todoItem.CreateAsync(todoDto.text, Id, usertype);

        //    return Ok();
        //}


        // GET: api/Todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
        {
            var todos = await _todoItem.GetAllAsync();
            return Ok(todos);
        }


        // DELETE: api/Todos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var deleted = await _todoItem.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT: api/Todos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, TodoItem todoItem)
        {
            if (id != todoItem._id)
            {
                return BadRequest();
            }

            var updated = await _todoItem.UpdateAsync(todoItem);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
