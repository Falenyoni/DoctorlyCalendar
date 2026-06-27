using DoctorlyCalendar.Domain.Entities;

namespace DoctorlyCalendar.Domain.Interfaces;

public interface INotificationService
{
    Task NotifyEventCreatedAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default);
    Task NotifyEventUpdatedAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default);
    Task NotifyEventCancelledAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default);
    Task NotifyAttendeeAddedAsync(CalendarEvent calendarEvent, Attendee attendee, CancellationToken cancellationToken = default);
}
