namespace Domain.DTOs;

public class TodoDto
{
    public TodoDto(int id, string ownerName, string title, bool completed)
    {
        Id = id;
        OwnerName = ownerName;
        Title = title;
        Completed = completed;
    }

    public int Id { get; }
    public string OwnerName { get; }
    public string Title { get; }
    public bool Completed { get; }
}