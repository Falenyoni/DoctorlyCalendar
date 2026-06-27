using System.ComponentModel.DataAnnotations;

namespace DoctorlyCalendar.Features.Events.CreateNewEvents;

public record AttendeeRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public Guid RowVersion { get; set; }
}