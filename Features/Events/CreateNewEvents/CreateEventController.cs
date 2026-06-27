using DoctorlyCalendar.Domain.Entities;
using DoctorlyCalendar.Domain.Interfaces;
using DoctorlyCalendar.Features.Events;
using DoctorlyCalendar.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DoctorlyCalendar.Features.Events.CreateNewEvents;

[ApiController]
[Route("api/events")]
public class CreateEventController(ICalendarEventRepository repository, INotificationService notificationService) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Create a new calendar event.")]
    [EndpointDescription("Creates a calendar event with optional attendees. Attendees can also be added later.")]
    [Tags("Events")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request, CancellationToken cancellationToken)
    {
        var calendarEvent = CalendarEvent.Create(
            request.Title,
            request.Description,
            request.StartTime,
            request.EndTime
        );

        foreach (var attendee in request.Attendees)
        {
            calendarEvent.AddAttendee(attendee.Name, attendee.Email);
        }

        await repository.AddAsync(calendarEvent, cancellationToken);
        await notificationService.NotifyEventCreatedAsync(calendarEvent, cancellationToken);

        var response = MapToResponse(calendarEvent);

        return StatusCode(201, response);
    }

    private static CalendarEventResponse MapToResponse(CalendarEvent e) => new()
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
    };
}
