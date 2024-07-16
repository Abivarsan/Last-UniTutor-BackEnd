
﻿using Microsoft.EntityFrameworkCore;
using UniTutor.DataBase;
using UniTutor.Interface;
using UniTutor.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class CommentRepository : IComment
{
    private readonly ApplicationDBContext _DBcontext;

    public CommentRepository(ApplicationDBContext DBcontext)
    {
        _DBcontext = DBcontext;
    }

    public async Task AddCommentAsync(Comment comment)
    {
        _DBcontext.Comments.Add(comment);
        await _DBcontext.SaveChangesAsync();
    }

    public async Task CreateCommentAsync(string commentText, DateTime time, int Id, string usertype)
    {
        // Set CreatedAt to local time
        var slstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Colombo");
        var slstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, slstTimeZone);

        var comment = new Comment
        {
            commentText = commentText,
            timestamp = slstTime,
            userType = usertype
        };

        // Set the appropriate ID based on the usertype
        if (usertype.Equals("Student", StringComparison.OrdinalIgnoreCase))
        {
            comment.stuId = Id;
        }
        else if (usertype.Equals("Tutor", StringComparison.OrdinalIgnoreCase))
        {
            comment.tutId = Id;
        }
        else
        {
            throw new ArgumentException("Invalid user type. Must be 'Student' or 'Tutor'.");
        }

        await AddCommentAsync(comment);
    }


    public IEnumerable<Comment> GetAllComments()
    {
        return _DBcontext.Comments
            .Include(c => c.Student)
            .Include(c => c.Tutor)
            .ToList();
    }
}

