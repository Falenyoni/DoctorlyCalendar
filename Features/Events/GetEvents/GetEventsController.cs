using DoctorlyCalendar.Domain.Enums;
using DoctorlyCalendar.Features.Events;
using DoctorlyCalendar.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DoctorlyCalendar.Features.Events.GetEvents;

[ApiController]
[Route("api/events")]
public class GetEventsController(ICalendarEventRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] DateTimeOffset? from,
        [FromQuery] DateTimeOffset? to,
        [FromQuery] EventStatus? status,
        CancellationToken cancellationToken)
    {
        var events = await repository.GetAllAsync(from, to, status, cancellationToken);

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
