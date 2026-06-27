using DoctorlyCalendar.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DoctorlyCalendar.Features.Events.DeclineEvent;

[ApiController]
[Route("api/events")]
public class DeclineEventController(ICalendarEventRepository repository) : ControllerBase
{
    [HttpPut("{eventId:guid}/attendees/{attendeeId:guid}/decline")]
    public async Task<IActionResult> Decline(Guid eventId, Guid attendeeId, CancellationToken cancellationToken)
    {
        var calendarEvent = await repository.GetByIdAsync(eventId, cancellationToken);

        if (calendarEvent is null)
            return NotFound($"Event with id '{eventId}' was not found.");

        var attendee = calendarEvent.Attendees.FirstOrDefault(a => a.Id == attendeeId);

        if (attendee is null)
            return NotFound($"Attendee with id '{attendeeId}' was not found.");

        attendee.Decline();

        await repository.UpdateAsync(calendarEvent, cancellationToken);

        return NoContent();
    }
}
