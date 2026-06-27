using DoctorlyCalendar.Features.Events;
using DoctorlyCalendar.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DoctorlyCalendar.Features.Events.GetEventsById;

[ApiController]
[Route("api/events")]
public class GetEventByIdController(ICalendarEventRepository repository) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var calendarEvent = await repository.GetByIdAsync(id, cancellationToken);

        if (calendarEvent is null)
            return NotFound($"Event with id '{id}' was not found.");

        var response = new CalendarEventResponse
        {
            Id = calendarEvent.Id,
            Title = calendarEvent.Title,
            Description = calendarEvent.Description,
            StartTime = calendarEvent.StartTime,
            EndTime = calendarEvent.EndTime,
            Status = calendarEvent.EventStatus.ToString(),
            CreatedAt = calendarEvent.CreatedAt,
            UpdatedAt = calendarEvent.UpdatedAt,
            Attendees = calendarEvent.Attendees.Select(a => new AttendeeResponse
            {
                Id = a.Id,
                Name = a.Name,
                Email = a.Email,
                AttendanceStatus = a.AttendenceStatus.ToString()
            }).ToList()
        };

        return Ok(response);
    }
}
