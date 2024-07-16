using Microsoft.AspNetCore.Mvc;
using UniTutor.DTO;
using UniTutor.Interface;
using UniTutor.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly IComment _comment;

    public CommentController(IComment comment)
    {
        _comment = comment;
    }

    [HttpPost("create/{Id}/{usertype}")]
    public async Task<IActionResult> CreateStudentComment(int Id,string usertype, [FromBody] CreateComment createComment)
    {

        await _comment.CreateCommentAsync(createComment.commentText, createComment.Date, Id,usertype);

        return Ok();
    }

    [HttpGet]
    public ActionResult<IEnumerable<Comment>> GetAllComments()
    {
        var comments = _comment.GetAllComments()

            .Select(c => new
            {
                c._id,
                c.commentText,
                c.timestamp,
                c.userType,
                // c.ProfileUrl,

                fullName = c.userType == "Student" ?
                           $"{c.Student.firstName} {c.Student.lastName}" :
                           $"{c.Tutor.firstName} {c.Tutor.lastName}",

                profileUrl = c.userType == "Student" ?
                           $"{c.Student.ProfileUrl} " :
                           $"{c.Tutor.ProfileUrl}"
            })
            .ToList();

        return Ok(comments);
    }
    [HttpGet("getforside")]
    public ActionResult<IEnumerable<Comment>> GetAllCommentsForSide()
    {
        var comments = _comment.GetAllComments()

            .Select(c => new
            {
                c._id,
                c.commentText,
                c.timestamp,
                c.userType,


                fullName = c.userType == "Student" ?
                           $"{c.Student.firstName} {c.Student.lastName}" :
                           $"{c.Tutor.firstName} {c.Tutor.lastName}",

                profileUrl = c.userType == "Student" ?
                           $"{c.Student.ProfileUrl} " :
                           $"{c.Tutor.ProfileUrl}"
            })
            .ToList();

        return Ok(comments);
    }
}
