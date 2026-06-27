using DoctorlyCalendar.Domain.Enums;

namespace DoctorlyCalendar.Domain.Entities;

public class Attendee
{
    private Attendee()
    { }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public Guid CalendarEventId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public AttendenceStatus AttendenceStatus
    {
        get; private set;
    }
}