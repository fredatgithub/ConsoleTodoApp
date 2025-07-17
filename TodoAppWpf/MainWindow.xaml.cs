using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;

namespace TodoAppWpf
{
  /// <summary>
  /// Logique d'interaction pour MainWindow.xaml
  /// </summary>
  public partial class MainWindow: Window
  {
    ObservableCollection<TaskItem> tasks = new ObservableCollection<TaskItem>();
    int nextId = 1;
    const string fileName = "tasks.txt";

    public MainWindow()
    {
      InitializeComponent();
      TasksListBox.ItemsSource = tasks;
      LoadTasks();
    }

    private void AddTask_Click(object sender, RoutedEventArgs e)
    {
      string desc = DescriptionTextBox.Text.Trim();

      if (string.IsNullOrEmpty(desc) || desc == "Description...")
      {
        return;
      }

      var task = new TaskItem
      {
        Id = nextId++,
        Description = desc,
        DueDate = DueDatePicker.SelectedDate,
        IsCompleted = false
      };
      tasks.Add(task);

      DescriptionTextBox.Text = "Description...";
      DescriptionTextBox.Foreground = System.Windows.Media.Brushes.Gray;
      DueDatePicker.SelectedDate = null;
    }

    private void DeleteTask_Click(object sender, RoutedEventArgs e)
    {
      if (TasksListBox.SelectedItem is TaskItem selected)
      {
        tasks.Remove(selected);
      }
    }

    private void ToggleTask_Click(object sender, RoutedEventArgs e)
    {
      if (TasksListBox.SelectedItem is TaskItem selected)
      {
        selected.IsCompleted = !selected.IsCompleted;
        // Rafraîchir l'affichage :
        TasksListBox.Items.Refresh();
      }
    }

    private void SaveTasks_Click(object sender, RoutedEventArgs e)
    {
      SaveTasksToFile();
    }

    private void SaveTasksToFile()
    {
      using (StreamWriter sw = new StreamWriter(fileName))
      {
        foreach (var task in tasks)
        {
          sw.WriteLine($"{task.Id}|{task.Description}|{task.DueDate?.ToString("o")}|{task.IsCompleted}");
        }
      }

      MessageBox.Show("Tâches sauvegardées !");
    }

    private void LoadTasks()
    {
      tasks.Clear();
      if (File.Exists(fileName))
      {
        foreach (var line in File.ReadAllLines(fileName))
        {
          var parts = line.Split('|');
          if (parts.Length == 4)
          {
            var task = new TaskItem
            {
              Id = int.Parse(parts[0]),
              Description = parts[1],
              DueDate = string.IsNullOrEmpty(parts[2]) ? (DateTime?)null : DateTime.Parse(parts[2], new CultureInfo("fr-FR")),
              IsCompleted = bool.Parse(parts[3])
            };
            tasks.Add(task);
            nextId = Math.Max(nextId, task.Id + 1);
          }
        }
      }
    }

    private void DescriptionTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      if (DescriptionTextBox.Text == "Description...")
      {
        DescriptionTextBox.Text = "";
        DescriptionTextBox.Foreground = System.Windows.Media.Brushes.Black;
      }
    }

    private void DescriptionTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
      {
        DescriptionTextBox.Text = "Description...";
        DescriptionTextBox.Foreground = System.Windows.Media.Brushes.Gray;
      }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      if (DueDatePicker.SelectedDate.HasValue)
      {
        MessageBox.Show("Date sélectionnée : " + DueDatePicker.SelectedDate.Value.ToShortDateString());
      }
      else
      {
        MessageBox.Show("Aucune date sélectionnée.");
      }
    }
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      SaveTasksToFile();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LoadTasks();
    }
  }
}
