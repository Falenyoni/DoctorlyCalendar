using DoctorlyCalendar.Domain.Enums;

namespace DoctorlyCalendar.Domain.Entities;

public class CalendarEvent
{
    private CalendarEvent() { }

    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset StartTime { get; private set; }
    public DateTimeOffset EndTime { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public EventStatus EventStatus { get; private set; }
    public Guid RowVersion { get; private set; }

    private readonly List<Attendee> _attendees = new();
    public IReadOnlyCollection<Attendee> Attendees => _attendees.AsReadOnly();

    public static CalendarEvent Create(string title, string description, DateTimeOffset startTime, DateTimeOffset endTime)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.", nameof(title));
        if (endTime <= startTime) throw new ArgumentException("End time must be after start time.");

        return new CalendarEvent
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description ?? string.Empty,
            StartTime = startTime,
            EndTime = endTime,
            EventStatus = EventStatus.Scheduled,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = Guid.NewGuid()
        };
    }

    public void Update(string title, string description, DateTimeOffset startTime, DateTimeOffset endTime)
    {
        if (EventStatus == EventStatus.Cancelled)
            throw new InvalidOperationException("Cannot update a cancelled event.");
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.", nameof(title));
        if (endTime <= startTime) throw new ArgumentException("End time must be after start time.");

        Title = title;
        Description = description ?? string.Empty;
        StartTime = startTime;
        EndTime = endTime;
        UpdatedAt = DateTimeOffset.UtcNow;
        RowVersion = Guid.NewGuid();
    }

    public void Cancel()
    {
        if (EventStatus == EventStatus.Cancelled)
            throw new InvalidOperationException("Event is already cancelled.");
        if (EventStatus == EventStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed event.");

        EventStatus = EventStatus.Cancelled;
        UpdatedAt = DateTimeOffset.UtcNow;
        RowVersion = Guid.NewGuid();
    }

    public void Complete()
    {
        if (EventStatus != EventStatus.Scheduled)
            throw new InvalidOperationException("Only scheduled events can be completed.");

        EventStatus = EventStatus.Completed;
        UpdatedAt = DateTimeOffset.UtcNow;
        RowVersion = Guid.NewGuid();
    }

    public void AddAttendee(string name, string email)
    {
        if (EventStatus == EventStatus.Cancelled)
            throw new InvalidOperationException("Cannot add attendees to a cancelled event.");
        if (_attendees.Any(a => a.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Attendee with email '{email}' is already added.");

        _attendees.Add(Attendee.Create(Id, name, email));
        UpdatedAt = DateTimeOffset.UtcNow;
        RowVersion = Guid.NewGuid();
    }
}
