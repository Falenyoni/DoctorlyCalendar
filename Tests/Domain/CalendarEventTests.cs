using DoctorlyCalendar.Domain.Entities;
using DoctorlyCalendar.Domain.Enums;
using Shouldly;

namespace DoctorlyCalendar.Tests.Domain;

public class CalendarEventTests
{
    private static readonly DateTimeOffset Start = DateTimeOffset.UtcNow.AddHours(1);
    private static readonly DateTimeOffset End = DateTimeOffset.UtcNow.AddHours(2);

    [Fact]
    public void Create_WithValidInputs_SetsScheduledStatus()
    {
        // Arrange & Act
        var calendarEvent = CalendarEvent.Create("Standup", "Daily sync", Start, End);

        // Assert
        calendarEvent.EventStatus.ShouldBe(EventStatus.Scheduled);
    }

    [Fact]
    public void Create_WhenEndTimeBeforeStartTime_ThrowsArgumentException()
    {
        // Arrange
        var act = () => CalendarEvent.Create("Standup", "Daily sync", End, Start);

        // Act & Assert
        Should.Throw<ArgumentException>(act)
            .Message.ShouldContain("End time");
    }

    [Fact]
    public void Cancel_WhenAlreadyCancelled_ThrowsInvalidOperationException()
    {
        // Arrange
        var calendarEvent = CalendarEvent.Create("Standup", "Daily sync", Start, End);
        calendarEvent.Cancel();

        // Act
        var act = () => calendarEvent.Cancel();

        // Assert
        Should.Throw<InvalidOperationException>(act)
            .Message.ShouldContain("already cancelled");
    }

    [Fact]
    public void AddAttendee_WithDuplicateEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        var calendarEvent = CalendarEvent.Create("Standup", "Daily sync", Start, End);
        calendarEvent.AddAttendee("Bongani Nyoni", "b@example.com");

        // Act
        var act = () => calendarEvent.AddAttendee("Bongani Nyoni", "b@example.com");

        // Assert
        Should.Throw<InvalidOperationException>(act)
            .Message.ShouldContain("already added");
    }

    [Fact]
    public void Update_WhenCancelled_ThrowsInvalidOperationException()
    {
        // Arrange
        var calendarEvent = CalendarEvent.Create("Standup", "Daily sync", Start, End);
        calendarEvent.Cancel();

        // Act
        var act = () => calendarEvent.Update("Updated", "desc", Start, End);

        // Assert
        Should.Throw<InvalidOperationException>(act)
            .Message.ShouldContain("cancelled");
    }
}
