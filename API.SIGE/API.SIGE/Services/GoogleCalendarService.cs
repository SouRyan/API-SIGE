namespace GerenciamentoProducao.Services;

/// <summary>
/// Integração opcional com Google Calendar; implementação mínima para permitir compilação e execução sem credenciais.
/// </summary>
public sealed class GoogleCalendarService
{
    public CalendarEventResult CreateEvent(
        string calendarId,
        string summary,
        DateTime start,
        DateTime end,
        string? description = null)
    {
        _ = calendarId;
        _ = summary;
        _ = start;
        _ = end;
        _ = description;
        return new CalendarEventResult { Id = Guid.NewGuid().ToString("N") };
    }

    public void DeleteEvent(string calendarId, string eventId)
    {
        _ = calendarId;
        _ = eventId;
    }
}

public sealed class CalendarEventResult
{
    public string Id { get; init; } = string.Empty;
}
