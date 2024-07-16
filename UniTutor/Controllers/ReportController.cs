
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniTutor.DTO;
using UniTutor.Interface;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReport([FromBody] CreateReportDto createReportDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var report = await _reportService.CreateReportAsync(createReportDto);
        return CreatedAtAction(nameof(GetReport), new { id = report._id }, report);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReport(int id)
    {
        var report = await _reportService.GetReportByIdAsync(id);

        if (report == null)
        {
            return NotFound();
        }

        return Ok(report);
    }

    [HttpGet("Al")]
    public async Task<IActionResult> GetAllReports()
    {
        var reports = await _reportService.GetAllReportsAsync();
        return Ok(reports);
    }

    [HttpPost("suspend")]
    public async Task<IActionResult> SuspendUser(int userId, string userType)
    {
        var result = await _reportService.SuspendUserAsync(userId, userType);
        if (!result) return BadRequest("User not found or invalid user type.");
        return Ok("User suspended successfully.");
    }

    [HttpPost("restore")]
    public async Task<IActionResult> RestoreUser(int userId, string userType)
    {
        var result = await _reportService.RestoreUserAsync(userId, userType);
        if (!result) return BadRequest("User not found or invalid user type.");
        return Ok("User restored successfully.");
    }
}











