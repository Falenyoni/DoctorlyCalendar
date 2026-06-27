using DoctorlyCalendar.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoctorlyCalendar.Infrastructure.Data.Repositories;

public class CalendarEventRepository(AppDbContext context) : ICalendarEventRepository
{
    public async Task<CalendarEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    => await context.CalendarEvents
        .Include(e => e.Attendees)
        .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<IEnumerable<CalendarEvent>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.CalendarEvents
            .Include(e => e.Attendees)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default)
    {
        await context.CalendarEvents.AddAsync(calendarEvent, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default)
    {
        context.CalendarEvents.Update(calendarEvent);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken = default)
    {
        context.CalendarEvents.Remove(calendarEvent);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<CalendarEvent>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        => await context.CalendarEvents
            .Include(e => e.Attendees)
            .Where(e => e.Title.Contains(searchTerm) || e.Description.Contains(searchTerm))
            .ToListAsync(cancellationToken);
}