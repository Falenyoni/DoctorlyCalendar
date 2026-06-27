using DoctorlyCalendar.Domain.Enums;

namespace DoctorlyCalendar.Domain.Entities;

public class CalendarEvent
{
    private CalendarEvent()
    { }

    public Guid Id { get; private set; }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset StartTime { get; private set; }
    public DateTimeOffset EndTime { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public EventStatus EventStatus { get; private set; }

    private readonly List<Attendee> _attendees = new();
    public IReadOnlyCollection<Attendee> Attendees => _attendees.AsReadOnly();

    public byte[] RowVersion
    {
        get; private set;
    }
}