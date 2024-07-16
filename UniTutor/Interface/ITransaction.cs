using UniTutor.Model;
namespace UniTutor.Interface

{
    public interface ITransaction
    {
        void AddTransaction(Transaction transaction);
        public  Task<IEnumerable<Transaction>> GetTransactionsByTutorId(int tutorId);
       
    }
}
