namespace Domain.DTOs;

public class SearchTodoParametersDto
{
    public string? UserName { get; }
    public int? UserId { get; }
    public bool? CompletedStatus { get; }
    public string? TitleContains { get; }

    public SearchTodoParametersDto(string? userName, int? userId, bool? completedStatus, string? titleContains)
    {
        UserName = userName;
        UserId = userId;
        CompletedStatus = completedStatus;
        TitleContains = titleContains;
    }
}