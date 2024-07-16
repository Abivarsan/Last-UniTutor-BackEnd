using System.Threading.Tasks;
using UniTutor.DTO;

namespace UniTutor.Interface
{
    public interface IPaymentService
    {
        public string CreateCheckoutSession(int userId, CreateCheckoutSessionDto createCheckoutSessionDto);
        void AddTransaction(TransactionDto transactionDto);
    }
}
