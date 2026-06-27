using DoctorlyCalendar.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DoctorlyCalendar.Features.Events.CancelEvent;

[ApiController]
[Route("api/events")]
public class CancelEventController(ICalendarEventRepository repository) : ControllerBase
{
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var calendarEvent = await repository.GetByIdAsync(id, cancellationToken);

        if (calendarEvent is null)
            return NotFound($"Event with id '{id}' was not found.");

        calendarEvent.Cancel();

        await repository.UpdateAsync(calendarEvent, cancellationToken);

        return NoContent();
    }
}
