using DoctorlyCalendar.Domain.Entities;
using DoctorlyCalendar.Domain.Enums;

namespace DoctorlyCalendar.Infrastructure.Data.Repositories;

public interface ICalendarEventRepository
{
    Task<CalendarEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<CalendarEvent>> GetAllAsync(DateTimeOffset? from, DateTimeOffset? to, EventStatus? status, CancellationToken cancellationToken = default);

    Task AddAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default);

    Task UpdateAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default);

    Task DeleteAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default);

    Task<IEnumerable<CalendarEvent>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}