using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UniTutor.DataBase;
using UniTutor.DTO;
using UniTutor.Interface;
using UniTutor.Model;

namespace UniTutor.Repository
{
    public class RequestRepository : IRequest
    {
        private ApplicationDBContext _DBcontext;
        private readonly IConfiguration _config;
      



        public RequestRepository(ApplicationDBContext DBcontext, IConfiguration config )
        {
            _DBcontext = DBcontext;
            _config = config;
            


        }
        public async Task<IEnumerable<Request>> GetAll()
        {
            return await _DBcontext.Requests.ToListAsync();
        }

        public async Task<Request> GetById(int id)
        {
            return await _DBcontext.Requests.FindAsync(id);
        }

        public async Task<IEnumerable<Request>> GetByStudentId(int studentId)
        {
            return await _DBcontext.Requests
                .Where(sr => sr.studentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Request>> GetByTutorId(int tutorId)
        {
            return await _DBcontext.Requests

                .Where(sr => sr.tutorId == tutorId &&(sr.status == "PENDING" || sr.status == "REJECTED")) // Adjust according to your entity's property
                .Include(sr => sr.Subject)
                .Include(sr => sr.Student)
                .ToListAsync();
        }

        public async Task<Request> Create(RequestDto request)
        {
            var slstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Colombo");
            var slstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, slstTimeZone);
            var srequest = new Request
            {
                studentId = request.studentId,
                subjectId = request.subjectId,
                tutorId = request.tutorId,
                studentEmail = request.studentEmail,
                // status = request.status ?? "PENDING",
                timestamp = slstTime
            };

            try
            {
                _DBcontext.Requests.Add(srequest);
                await _DBcontext.SaveChangesAsync();
                return srequest;
            }
            catch (Exception ex)
            {
                // Log the exception here
                throw new Exception("An error occurred while creating the request.", ex);
            }
        }

        //public async Task<Request> UpdateRequestStatus(int id, string status)
        //{
        //    var request = await _DBcontext.Requests.FindAsync(id);
        //    if (request == null)
        //    {
        //        return null;
        //    }

        //    request.status = status;
        //    await _DBcontext.SaveChangesAsync();

        //    return request;
        //}

        public async Task<Request> UpdateRequestStatus(int id, string status)
        {
            using (var transaction = await _DBcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var request = await _DBcontext.Requests.FindAsync(id);
                    if (request == null)
                    {
                        Console.WriteLine("Request not found.");
                        return null; // or throw an exception as per your application's error handling strategy
                    }

                    // Logging the current status of the request
                    Console.WriteLine($"Current request status: {request.status}");
                    request.status = status;
                    Console.WriteLine($"Updated request status to: {status}");

                    _DBcontext.Requests.Update(request);

                    if (status == "ACCEPTED")
                    {
                        var tutor = await _DBcontext.Tutors.FindAsync(request.tutorId);
                        if (tutor != null)
                        {
                            // Add transaction record
                            var transactionRecord = new Transaction
                            {
                                tutorId = tutor._id,
                                Coins = -20, // Adjust as per your business logic
                                timestamp = DateTime.UtcNow,
                                Description = "Paid for accept student",
                                StripeSessionId = ""
                            };
                            _DBcontext.Transactions.Add(transactionRecord);

                            Console.WriteLine("Transaction record added.");
                        }
                        else
                        {
                            Console.WriteLine("Tutor not found.");
                            // Handle scenario where tutor is not found
                        }
                    }

                    await _DBcontext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    Console.WriteLine("Transaction committed.");
                    return (request);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine("Transaction rolled back. Error: " + ex.Message);

                    // Logging inner exception details
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    }

                    throw; // Propagate the exception to the calling method or controller
                }
            }
        }




        public async Task<Request> Delete(int id)
        {
            var request = await _DBcontext.Requests.FindAsync(id);
            if (request == null)
            {
                return null;
            }

            _DBcontext.Requests.Remove(request);
            await _DBcontext.SaveChangesAsync();
            return request;
        }
       
       //get all requests by tutor id
        public async Task<IEnumerable<Request>> GetAcceptedRequestsByTutorId(int tutorId)
        {
            return await _DBcontext.Requests
                .Where(sr => sr.tutorId == tutorId && sr.status == "ACCEPTED")
                .Include(sr => sr.Subject)
                .Include(sr => sr.Student)
                .Include(sr => sr.Tutor)
                .ToListAsync();
        }
        //get all requests by student id
        public async Task<IEnumerable<Request>> GetAcceptedRequestsByStudentId(int studentId)
        {
            return await _DBcontext.Requests
                .Where(sr => sr.studentId == studentId && sr.status == "ACCEPTED")
                .Include(sr => sr.Subject)
                .Include(sr => sr.Student)
                .Include(sr => sr.Tutor)
                .ToListAsync();
        }
        //get all request by student id where statues pending and rejected
        public async Task<IEnumerable<Request>> GetAllRequestsByStudentId(int studentId)
        {
            return await _DBcontext.Requests
                .Where(sr => sr.studentId == studentId && (sr.status == "PENDING" || sr.status == "REJECTED"))
                .Include(sr => sr.Subject)
                .Include(sr => sr.Student)
                .Include(sr => sr.Tutor)
                .ToListAsync();
        }
       
        public async Task<int> GetMySubjectsCount(int studentId)
        {
            return await _DBcontext.Requests
                                 .Where(sr => sr.studentId == studentId && sr.status=="PENDING")
                                 .CountAsync();
        }

        public async Task<int> GetAcceptedRequestsCount(int studentId)
        {
            return await _DBcontext.Requests
                                 .Where(sr => sr.studentId == studentId && sr.status == "ACCEPTED")
                                 .CountAsync();
        }

        public async Task<int> GetRejectedRequestsCount(int studentId)
        {
            return await _DBcontext.Requests
                                 .Where(sr => sr.studentId == studentId && sr.status == "REJECTED")
                                 .CountAsync();
        }

        public async Task<int> GetMySubjectsCountTutor(int tutorId)
        {
            return await _DBcontext.Requests
                                 .Where(sr => sr.tutorId == tutorId)
                                 .CountAsync();
        }

        public async Task<int> GetMyStudentCount(int tutorId)
        {
            return await _DBcontext.Requests
                .Where(sr => sr.tutorId == tutorId && sr.status == "ACCEPTED")
                .CountAsync();
        }

        public async Task<int> GetAllRequestscount(int tutorId)
        {
            return await _DBcontext.Requests
                .Where(sr => sr.tutorId == tutorId && sr.status == "PENDING")
                .CountAsync();
        }

    }

}

