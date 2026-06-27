# DoctorlyCalendar

Backend API for managing calendar events and appointments within a doctor's practice.




## How To Run

- Clone the repo
- Open in Visual Studio or run `dotnet run` from the project root
- Swagger UI available at `https://localhost:{port}/swagger`
- No database setup needed - SQLite database is created automatically on first run
- To run tests: In Visual Studion run All Tests `dotnet test Tests/DoctorlyCalendar.Tests.csproj`


## Requirements

- Attendees must have a Name, Email Address, and attendance status (Pending / Accepted / Declined)
- Events must have a Title, Description, Start Time, End Time, and a list of Attendees
- Field sizes are limited (Title: 200 chars, Description: 2000 chars, Email: 254 chars)
- Events have a status: Scheduled, Cancelled, Completed
- Notifications capability must exist (currently stubbed with logging)
- Data must be persisted
- Solution must use DDD patterns and be properly layered


## API Functions

Events
- `POST /api/events` - Create a new event, optionally with attendees
- `GET /api/events` - List all events with optional filters (`?from=`, `?to=`, `?status=`)
- `GET /api/events/{id}` - Get a single event by ID
- `GET /api/events/search?term=` - Search events by title or description
- `PUT /api/events/{id}` - Update an event (requires RowVersion for concurrency)
- `DELETE /api/events/{id}` - Cancel an event

Attendees
- `POST /api/events/{id}/attendees` - Add an attendee to an existing event
- `PUT /api/events/{eventId}/attendees/{attendeeId}/accept` - Accept an event invitation
- `PUT /api/events/{eventId}/attendees/{attendeeId}/decline` - Decline an event invitation



## Key Decisions

- DDD / Rich Domain Model - `CalendarEvent` and `Attendee` have private setters. State changes go through domain methods (`Create`, `Update`, `Cancel`, `AddAttendee`, `Accept`, `Decline`). Guards and business rules live on the entity, not in controllers
- Attendee is not an aggregate root - Attendees are created exclusively through `CalendarEvent.AddAttendee()`. An attendee without an event has no meaning in this domain
- Optimistic concurrency via RowVersion - Every event carries a `RowVersion` (Guid). Write operations (Update, AddAttendee) check the client's RowVersion against the database before saving. A mismatch returns `409 Conflict`
- Vertical slice structure - Each operation lives in its own folder under `Features/Events/`. One controller, one responsibility, one reason to change
- No MediatR - At this scale MediatR adds ceremony without value. Controllers call the repository directly. If the app grew, handlers would move into a dedicated application layer
- Notification interface in Domain - `INotificationService` is defined in the Domain layer. The stub implementation (`LoggingNotificationService`) lives in Infrastructure. Swapping to email or a message queue requires no changes to domain or controllers
- SQLite for persistence - Chosen for simplicity and portability. No database server required to run the solution
- ICalendarEventRepository in Infrastructure - Ideally this interface belongs in Domain so the domain defines its own contract. Kept in Infrastructure here as a pragmatic call to avoid extra project setup within the time limit

---

## Improvements

- Switch to SQL Server or PostgreSQL - SQLite has limitations (no native `DateTimeOffset` ORDER BY, no `rowversion` type). A production database would remove these workarounds
- Native RowVersion - Replace the Guid concurrency token with a SQL Server `rowversion` (`byte[]`) or PostgreSQL `xmin`. The database would then manage the token automatically instead of the application
- Move ICalendarEventRepository to Domain - Proper DDD structure has the domain defining its own repository contracts, with Infrastructure implementing them
- Real notification implementation - Replace `LoggingNotificationService` with an email sender (SendGrid, SMTP), iCal generator, or a message queue (RabbitMQ, Azure Service Bus)
- Separate class libraries — Domain, Application, and Infrastructure as separate `.csproj` files would enforce layer boundaries at compile time instead of relying on convention
- Authentication and authorisation - No auth is implemented. A real system would need identity (who is the doctor, who is the patient) before allowing mutations
- Pagination - The `GET /api/events` endpoint returns all records. In production this needs pagination
- Soft delete - Cancelled events are flagged via status but not removed. An `IsDeleted` filter could hide them from default queries
-Adding Serilog 
- Add more domain Tests
- Add DomainExceptions
- Use private paremeterised constructors for Domain Entities
