using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ConsoleTodoApp
{
  internal static class Program
  {
    private static List<TaskItem> tasks = new List<TaskItem>();
    private static int nextId = 1;
    private const string fileName = "tasks.txt";

    static void Main()
    {
      LoadTasks();

      while (true)
      {
        Console.Clear();
        ShowMenu();

        Console.Write("Choix : ");
        string input = Console.ReadLine();

        switch (input)
        {
          case "1":
            ListTasks();
            break;
          case "2":
            AddTask();
            break;
          case "3":
            RemoveTask();
            break;
          case "4":
            ToggleTaskCompletion();
            break;
          case "5":
            SaveTasks();
            break;
          case "6":
            LoadTasks();
            break;
          case "0":
            return;
          default:
            Console.WriteLine("Choix invalide.");
            break;
        }

        Console.WriteLine("\nAppuie sur une touche pour continuer...");
        Console.ReadKey();
      }
    }

    static void ShowMenu()
    {
      Console.WriteLine("=== GESTIONNAIRE DE TÂCHES ===");
      Console.WriteLine("1. Afficher les tâches");
      Console.WriteLine("2. Ajouter une tâche");
      Console.WriteLine("3. Supprimer une tâche");
      Console.WriteLine("4. Marquer une tâche comme faite/non faite");
      Console.WriteLine("5. Sauvegarder les tâches");
      Console.WriteLine("6. Charger les tâches");
      Console.WriteLine("0. Quitter");
      Console.WriteLine("==============================");
    }

    static void ListTasks()
    {
      if (tasks.Count == 0)
      {
        Console.WriteLine("Aucune tâche.");
        return;
      }

      foreach (var task in tasks)
      {
        string status = task.IsCompleted ? "[X]" : "[ ]";
        Console.WriteLine($"{task.Id}. {status} {task.Description} {(task.DueDate.HasValue ? $"(Échéance: {task.DueDate.Value:dd/MM/yyyy})" : "")}");
      }
    }

    private static void AddTask()
    {
      Console.Write("Description : ");
      string desc = Console.ReadLine();

      Console.Write("Date d’échéance (jj/mm/aaaa) ou vide : ");
      string dateStr = Console.ReadLine();
      DateTime? dueDate = null;

      var culture = CultureInfo.GetCultureInfo("fr-FR");
      if (DateTime.TryParse(dateStr, culture, DateTimeStyles.None, out DateTime dt))
      {
        dueDate = dt;
      }

      tasks.Add(new TaskItem { Id = nextId++, Description = desc, DueDate = dueDate, IsCompleted = false });
      Console.WriteLine("Tâche ajoutée !");
    }

    private static void RemoveTask()
    {
      Console.Write("ID de la tâche à supprimer : ");
      if (int.TryParse(Console.ReadLine(), out int id))
      {
        tasks.RemoveAll(t => t.Id == id);
        Console.WriteLine("Tâche supprimée.");
      }
      else
      {
        Console.WriteLine("ID invalide.");
      }
    }

    private static void ToggleTaskCompletion()
    {
      Console.Write("ID de la tâche à modifier : ");
      if (int.TryParse(Console.ReadLine(), out int id))
      {
        var task = tasks.Find(t => t.Id == id);
        if (task != null)
        {
          task.IsCompleted = !task.IsCompleted;
          Console.WriteLine("Statut mis à jour.");
        }
        else
        {
          Console.WriteLine("Tâche introuvable.");
        }
      }
    }

    private static void SaveTasks()
    {
      using (StreamWriter sw = new StreamWriter(fileName))
      {
        foreach (var task in tasks)
        {
          sw.WriteLine($"{task.Id}|{task.Description}|{task.DueDate?.ToString("o")}|{task.IsCompleted}");
        }
      }

      Console.WriteLine("Tâches sauvegardées !");
    }

    private static void LoadTasks()
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
  }
}
