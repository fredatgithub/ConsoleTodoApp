using System;

namespace ConsoleTodoApp
{
  public class TaskItem
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
  }
}
