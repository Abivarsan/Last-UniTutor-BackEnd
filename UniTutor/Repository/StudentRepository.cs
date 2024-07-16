using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniTutor.DataBase;
using UniTutor.DTO;
using UniTutor.Interface;
using UniTutor.Model;

namespace UniTutor.Repository
{
    public class StudentRepository : IStudent
    {
        private ApplicationDBContext _DBcontext;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
       
        public StudentRepository(ApplicationDBContext DBcontext , IConfiguration config,IMapper mapper)
        {
            _DBcontext = DBcontext;
            _config = config;
            _mapper = mapper;
            
        }

        public bool SignUp(Student student)
        {
            try
            {
                _DBcontext.Students.Add(student);
                _DBcontext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public bool Login(string email, string password)
        {
            try
            {
                var student = _DBcontext.Students.FirstOrDefault(a => a.email == email);

                if (student == null)
                {
                    Console.WriteLine("Student not found.");
                    return false;
                }

                PasswordHash ph = new PasswordHash();

                bool isValidPassword = ph.VerifyPassword(password, student.password);

                return isValidPassword;
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"InvalidCastException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General exception: {ex.Message}");
                throw;
            }
        }

        public Student GetByMail(string Email)
        {
            return _DBcontext.Students.FirstOrDefault(s => s.email == Email);
        }
        public Student GetById(int id)
        {
            return _DBcontext.Students.Find(id);
        }
        public bool Delete(int id)
        {
            var student = _DBcontext.Students.Find(id);
            if (student != null)
            {
                _DBcontext.Students.Remove(student);
                _DBcontext.SaveChanges();
                return true;
            }
            return false;
        } 
        public bool SignOut()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    
        
        public bool DeleteRequest(Request request) 
        {
            try
            {
                _DBcontext.Requests.Remove(request);
                _DBcontext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public ICollection<Tutor> GetAllTutor()
        {
            var tutors = _DBcontext.Tutors.ToList();
            return tutors;
        }
      

        public async Task<bool> Update(Student student)
        {
            _DBcontext.Students.Update(student);
            return await _DBcontext.SaveChangesAsync() > 0;
        }

        public bool StudentExists(int id)
        {
            return _DBcontext.Students.Any(e => e._id == id);
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _DBcontext.Students.FindAsync(id);
            if (student != null)
            {
                _DBcontext.Students.Remove(student);
                await _DBcontext.SaveChangesAsync();
            }
        }
        public async Task UpdateAsync(Student student)
        {
            _DBcontext.Students.Update(student);
            await _DBcontext.SaveChangesAsync();
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _DBcontext.Students.FindAsync(id);
        }

        public async Task<StudentDashboardDeatilsDto> GetStudentDashboardDetails(int studentId)
        {
            var student = await _DBcontext.Students
                .Where(s => s._id == studentId)
                .Select(s => new StudentDashboardDeatilsDto
                {
                    firstName = s.firstName,
                    lastName = s.lastName,
                    email = s.email,
                    phoneNumber = s.phoneNumber,
                    ProfileUrl = s.ProfileUrl,
                })
                .FirstOrDefaultAsync();

            return student;
        }


    }
}
