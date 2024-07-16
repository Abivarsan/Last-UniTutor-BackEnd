using Microsoft.EntityFrameworkCore;
using UniTutor.DataBase;
using UniTutor.Interface;
using UniTutor.Model;

namespace UniTutor.Repository
{
    public class TransactionRepository : ITransaction
    {
        private readonly ApplicationDBContext _DBcontext;

        public TransactionRepository(ApplicationDBContext context)
        {
            _DBcontext = context;
        }

        public void AddTransaction(Transaction transaction)
        {
            _DBcontext.Transactions.Add(transaction);
            _DBcontext.SaveChanges();
        }
        public async Task<IEnumerable<Transaction>> GetTransactionsByTutorId(int tutorId)
        {
            return await _DBcontext.Transactions
                .Where(t => t.tutorId == tutorId)
                .ToListAsync();
        }
        
    }
}
