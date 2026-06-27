using DoctorlyCalendar.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoctorlyCalendar.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<CalendarEvent> CalendarEvents { get; set; }
    public DbSet<Attendee> Attendees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}