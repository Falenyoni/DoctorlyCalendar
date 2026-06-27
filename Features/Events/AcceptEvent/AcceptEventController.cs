using DoctorlyCalendar.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DoctorlyCalendar.Features.Events.AcceptEvent;

[ApiController]
[Route("api/events")]
public class AcceptEventController(ICalendarEventRepository repository) : ControllerBase
{
    [HttpPut("{eventId:guid}/attendees/{attendeeId:guid}/accept")]
    public async Task<IActionResult> Accept(Guid eventId, Guid attendeeId, CancellationToken cancellationToken)
    {
        var calendarEvent = await repository.GetByIdAsync(eventId, cancellationToken);

        if (calendarEvent is null)
            return NotFound($"Event with id '{eventId}' was not found.");

        var attendee = calendarEvent.Attendees.FirstOrDefault(a => a.Id == attendeeId);

        if (attendee is null)
            return NotFound($"Attendee with id '{attendeeId}' was not found.");

        attendee.Accept();

        await repository.UpdateAsync(calendarEvent, cancellationToken);

        return NoContent();
    }
}
