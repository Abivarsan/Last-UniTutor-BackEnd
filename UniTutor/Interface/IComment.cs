using UniTutor.Model;

namespace UniTutor.Interface
{
    public interface IComment
    {
       Task CreateCommentAsync(string commentText, DateTime createdAt, int Id,string usertype);
        Task AddCommentAsync(Comment comment);
        public IEnumerable<Comment> GetAllComments();


    }
}

