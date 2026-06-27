using DoctorlyCalendar.Features.Events;
using DoctorlyCalendar.Features.Events.CreateNewEvents;
using DoctorlyCalendar.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DoctorlyCalendar.Features.Events.AddAttendee;

[ApiController]
[Route("api/events")]
public class AddAttendeeController(ICalendarEventRepository repository) : ControllerBase
{
    [HttpPost("{id:guid}/attendees")]
    public async Task<IActionResult> AddAttendee(Guid id, [FromBody] AttendeeRequest request, CancellationToken cancellationToken)
    {
        var calendarEvent = await repository.GetByIdAsync(id, cancellationToken);

        if (calendarEvent is null)
            return NotFound($"Event with id '{id}' was not found.");

        calendarEvent.AddAttendee(request.Name, request.Email);

        await repository.UpdateAsync(calendarEvent, cancellationToken);

        var newAttendee = calendarEvent.Attendees
            .First(a => a.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)
                     && a.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));

        var response = new AttendeeResponse
        {
            Id = newAttendee.Id,
            Name = newAttendee.Name,
            Email = newAttendee.Email,
            AttendanceStatus = newAttendee.AttendenceStatus.ToString()
        };

        return StatusCode(201, response);
    }
}
