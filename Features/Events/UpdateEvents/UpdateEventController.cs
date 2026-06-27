using DoctorlyCalendar.Features.Events;
using DoctorlyCalendar.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DoctorlyCalendar.Features.Events.UpdateEvents;

[ApiController]
[Route("api/events")]
public class UpdateEventController(ICalendarEventRepository repository) : ControllerBase
{
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEventRequest request, CancellationToken cancellationToken)
    {
        var calendarEvent = await repository.GetByIdAsync(id, cancellationToken);

        if (calendarEvent is null)
            return NotFound($"Event with id '{id}' was not found.");

        if (calendarEvent.RowVersion != request.RowVersion)
            return Conflict("The event was modified by someone else. Please reload and try again.");

        calendarEvent.Update(request.Title, request.Description, request.StartTime, request.EndTime);

        await repository.UpdateAsync(calendarEvent, cancellationToken);

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
