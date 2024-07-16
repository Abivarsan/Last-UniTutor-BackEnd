using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using UniTutor.DataBase;
using UniTutor.DTO;
using UniTutor.Interface;
using UniTutor.Model;

namespace UniTutor.Repository
{
    public class TutorRepository : ITutor
    {
        private ApplicationDBContext _DBcontext;
        private readonly IConfiguration _config;
       


        public TutorRepository(ApplicationDBContext DBcontext, IConfiguration config)
        {
            _DBcontext = DBcontext;
            _config = config;
        }
        



        public bool SignUp(Tutor tutor)
        {
            try
            {
                _DBcontext.Tutors.Add(tutor);
                _DBcontext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        
        public bool login(string email, string password)
        {
            var tutor = _DBcontext.Tutors.FirstOrDefault(a => a.universityMail == email);

            if (tutor == null)
            {
                return false;
            }

            PasswordHash ph = new PasswordHash();

            bool isValidPassword = ph.VerifyPassword(password, tutor.password);
            Console.WriteLine($"Password Validation : {isValidPassword}");

            if (isValidPassword)
            {
              
                return true;
            }
            else
            {
                return false;
            }
        }

        public Tutor GetTutorByEmail(string email)
        {
            return _DBcontext.Tutors.FirstOrDefault(x => x.universityMail == email);
        }
        public bool isUser(string email)
        {
            throw new NotImplementedException();
        }

        public bool logout()
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
        public bool Delete(int id)
        {
            var tutor = _DBcontext.Tutors.Find(id);
            if (tutor != null)
            {
                _DBcontext.Tutors.Remove(tutor);
                _DBcontext.SaveChanges();
                return true;
            }
            return false;
        }

        public Tutor GetById(int id)
        {
            return _DBcontext.Tutors.Find(id);
        }

        public IEnumerable<Tutor> GetAll()
        {
            return _DBcontext.Tutors.ToList();
        }
        public bool Updatetutor(int id)
        {
            var tutor = _DBcontext.Tutors.Find(id);
            if (tutor != null)
            {
                _DBcontext.Tutors.Update(tutor);
                _DBcontext.SaveChanges();
                return true;

            }
            return false;

        }
       
        
        public async Task<Tutor> GetTutorAsync(int id)
        {
            return await _DBcontext.Tutors.FindAsync(id);
        }

        public async Task UpdateAsync(Tutor tutor)
        {
            _DBcontext.Tutors.Update(tutor);
            await _DBcontext.SaveChangesAsync();
        }

        public async Task<Tutor> GetByIdAsync(int id)
        {
            return await _DBcontext.Tutors.FindAsync(id);
        }

        public async Task<TutorDashboardDetailsDto> GetTutorDashboardDetails(int tutorId)
        {
            var tutor = await _DBcontext.Tutors
                .Where(s => s._id == tutorId)
                .Select(s => new TutorDashboardDetailsDto
                {
                    firstName = s.firstName,
                    lastName = s.lastName,
                    universityMail=s.universityMail,
                    phoneNumber = s.phoneNumber,
                    ProfileUrl = s.ProfileUrl,
                    occupation = s.occupation,
                    address = s.address,
                    district = s.district,
                    qualifications = s.qualifications
                    
                })
                .FirstOrDefaultAsync();

            return tutor;
        }




    }

}
