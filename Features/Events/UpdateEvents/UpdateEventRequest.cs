using System.ComponentModel.DataAnnotations;

namespace DoctorlyCalendar.Features.Events.UpdateEvents;

public record UpdateEventRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; init; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; init; } = string.Empty;

    [Required]
    public DateTimeOffset StartTime { get; init; }

    [Required]
    public DateTimeOffset EndTime { get; init; }

    [Required]
    public Guid RowVersion { get; init; }
}