using DoctorlyCalendar.Domain.Entities;

namespace DoctorlyCalendar.Infrastructure.Data.Repositories;

public interface ICalendarEventRepository
{
    Task<CalendarEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<CalendarEvent>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default);

    Task UpdateAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default);

    Task DeleteAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default);

    Task<IEnumerable<CalendarEvent>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}