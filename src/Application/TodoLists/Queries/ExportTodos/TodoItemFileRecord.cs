using MediaLink.Application.Common.Mappings;
using MediaLink.Domain.Entities;

namespace MediaLink.Application.TodoLists.Queries.ExportTodos;

public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; set; }

    public bool Done { get; set; }
}
