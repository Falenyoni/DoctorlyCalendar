using DoctorlyCalendar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorlyCalendar.Infrastructure.Data.Configurations;

public class AttendeeConfiguration : IEntityTypeConfiguration<Attendee>
{
    public void Configure(EntityTypeBuilder<Attendee> builder)
    {
        builder.ToTable("Attendees");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Email)
            .HasMaxLength(1000);

        builder.Property(a => a.AttendenceStatus)
                    .HasConversion<string>()
                    .HasMaxLength(20);

        builder.Property(a => a.CalendarEventId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();
    }
}