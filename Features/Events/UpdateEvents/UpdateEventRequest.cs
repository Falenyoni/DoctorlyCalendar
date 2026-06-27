using System.ComponentModel.DataAnnotations;

namespace DoctorlyCalendar.Features.Events.UpdateEvents;

public class UpdateEventRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTimeOffset StartTime { get; set; }

    [Required]
    public DateTimeOffset EndTime { get; set; }
}