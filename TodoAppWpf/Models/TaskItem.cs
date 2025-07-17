using System;
using System.ComponentModel;

namespace TodoAppWpf
{
  public class TaskItem : INotifyPropertyChanged
  {
    private bool _isCompleted;

    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }

    public bool IsCompleted
    {
      get => _isCompleted;
      set
      {
        if (_isCompleted != value)
        {
          _isCompleted = value;
          OnPropertyChanged(nameof(IsCompleted));
          OnPropertyChanged(nameof(Display));
        }
      }
    }

    public string Display => $"{(IsCompleted ? "[X]" : "[ ]")} {Description} {(DueDate.HasValue ? $"(Échéance: {DueDate.Value:dd/MM/yyyy})" : "")}";

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
  }
}
