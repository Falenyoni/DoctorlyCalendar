using DoctorlyCalendar.Domain.Entities;
using DoctorlyCalendar.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DoctorlyCalendar.Infrastructure.Notifications;

public class LoggingNotificationService(ILogger<LoggingNotificationService> logger) : INotificationService
{
    public Task NotifyEventCreatedAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Event created: {EventId} - {Title}", calendarEvent.Id, calendarEvent.Title);
        return Task.CompletedTask;
    }

    public Task NotifyEventUpdatedAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Event updated: {EventId} - {Title}", calendarEvent.Id, calendarEvent.Title);
        return Task.CompletedTask;
    }

    public Task NotifyEventCancelledAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Event cancelled: {EventId} - {Title}", calendarEvent.Id, calendarEvent.Title);
        return Task.CompletedTask;
    }

    public Task NotifyAttendeeAddedAsync(CalendarEvent calendarEvent, Attendee attendee, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Attendee added to event {EventId}: {AttendeeName} ({AttendeeEmail})",
            calendarEvent.Id, attendee.Name, attendee.Email);
        return Task.CompletedTask;
    }
}
