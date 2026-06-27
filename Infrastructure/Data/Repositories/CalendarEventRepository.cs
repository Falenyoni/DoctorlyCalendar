using DoctorlyCalendar.Domain.Entities;
using DoctorlyCalendar.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DoctorlyCalendar.Infrastructure.Data.Repositories;

public class CalendarEventRepository(AppDbContext context) : ICalendarEventRepository
{
    public async Task<CalendarEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    => await context.CalendarEvents
        .Include(e => e.Attendees)
        .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<IEnumerable<CalendarEvent>> GetAllAsync(DateTimeOffset? from, DateTimeOffset? to, EventStatus? status, CancellationToken cancellationToken = default)
    {
        var query = context.CalendarEvents.Include(e => e.Attendees).AsQueryable();

        if (from.HasValue)
            query = query.Where(e => e.StartTime >= from.Value);

        if (to.HasValue)
            query = query.Where(e => e.EndTime <= to.Value);

        if (status.HasValue)
            query = query.Where(e => e.EventStatus == status.Value);

        var results = await query.ToListAsync(cancellationToken);
        return results.OrderBy(e => e.StartTime);
    }

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