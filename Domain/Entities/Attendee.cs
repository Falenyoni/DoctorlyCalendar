using DoctorlyCalendar.Domain.Enums;

namespace DoctorlyCalendar.Domain.Entities;

public class Attendee
{
    private Attendee() { }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public Guid CalendarEventId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public AttendenceStatus AttendenceStatus { get; private set; }

    internal static Attendee Create(Guid calendarEventId, string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.", nameof(email));

        return new Attendee
        {
            Id = Guid.NewGuid(),
            CalendarEventId = calendarEventId,
            Name = name,
            Email = email,
            AttendenceStatus = AttendenceStatus.Pending,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Accept()
    {
        if (AttendenceStatus == AttendenceStatus.Accepted)
            throw new InvalidOperationException("Attendee has already accepted.");

        AttendenceStatus = AttendenceStatus.Accepted;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Decline()
    {
        if (AttendenceStatus == AttendenceStatus.Declined)
            throw new InvalidOperationException("Attendee has already declined.");

        AttendenceStatus = AttendenceStatus.Declined;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
