using DoctorlyCalendar.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DoctorlyCalendar.Features.Events.SearchEvents;

[ApiController]
[Route("api/events")]
public class SearchEventsController(ICalendarEventRepository repository) : ControllerBase
{
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string term, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(term))
            return BadRequest("Search term is required.");

        var events = await repository.SearchAsync(term, cancellationToken);

        var response = events.Select(e => new CalendarEventResponse
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            Status = e.EventStatus.ToString(),
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt,
            Attendees = e.Attendees.Select(a => new AttendeeResponse
            {
                Id = a.Id,
                Name = a.Name,
                Email = a.Email,
                AttendanceStatus = a.AttendenceStatus.ToString()
            }).ToList()
        });

        return Ok(response);
    }
}