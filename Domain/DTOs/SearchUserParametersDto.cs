namespace Domain.DTOs;

public class SearchUserParametersDto
{
    public string? UserNameContains { get; }

    public SearchUserParametersDto(string? userNameContains)
    {
        UserNameContains = userNameContains;
    }
}