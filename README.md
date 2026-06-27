# DoctorlyCalendar

Calendar event management API built as part of a technical assessment.

## How to run

```
dotnet run
```

Swagger at `https://localhost:{port}/swagger`. Database is SQLite and gets created automatically on first run.

## What's in here

- Create calendar events with optional attendees in one request
- Add attendees to an existing event after the fact
- Attendees can accept or decline
- Events can be cancelled or completed
- Duplicate attendee emails on the same event are blocked
- You can't update or add attendees to a cancelled event

Get/Search/Cancel/Complete endpoints still to be added.

## Stack

- .NET 8, ASP.NET Core
- Entity Framework Core + SQLite (migrations run on startup)
- No MediatR — handlers are just controllers for simplicity at this scale

## Structure

```
Features/        <- one folder per operation (vertical slice)
Domain/          <- entities and enums, no infrastructure dependencies
Infrastructure/  <- EF Core, repository implementations
```

## Decisions worth noting

**Rich domain model** — `CalendarEvent` and `Attendee` have private setters. You can't set `Title = "x"` from outside. You go through `Create()`, `Update()`, `Cancel()` etc. Guards live on the entity, not scattered across controllers.

**Attendee is not an aggregate root** — it can only be created through `CalendarEvent.AddAttendee()`. A standalone `POST /api/attendees` wouldn't make sense because an attendee without an event is meaningless.

**RowVersion is a Guid** — normally this would be a SQL Server `rowversion` (byte[]). SQLite doesn't support that, so it's a `Guid` we generate ourselves on every write. Same optimistic concurrency idea, different mechanism.

**ICalendarEventRepository is in Infrastructure** — ideally it lives in Domain so the domain defines its own contract. Kept it in Infrastructure here as a pragmatic call to avoid extra project setup for an assessment. Worth fixing in a real codebase.

**No MediatR** — didn't add it because at this scale it adds ceremony without value. Controllers talk directly to the repository. If the app grew, the handlers would move into a service/application layer.
