using System;
using System.Collections.Generic;
using System.IO;

namespace TaskListDone
{
    class Program
    {
        static Dictionary<string, List<string>> tasksDict = new Dictionary<string, List<string>>();
        static string filePath;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Task List!");

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("What action would you like to perform?");
                Console.WriteLine("1. File actions");
                Console.WriteLine("2. Exit");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        FileActionsMenu();
                        break;
                    case "2":
                        SaveAllTasksToFile();
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please select a valid option.");
                        break;
                }
            }
        }

        static void LoadTasksFromFile(string file)
        {
            if (File.Exists(file))
            {
                tasksDict[file] = new List<string>(File.ReadAllLines(file));
            }
        }

        static void SaveTasksToFile(string file)
        {
            if (!string.IsNullOrEmpty(file))
            {
                File.WriteAllLines(file, tasksDict[file]);
            }
        }

        static void SaveAllTasksToFile()
        {
            foreach (var file in tasksDict.Keys)
            {
                SaveTasksToFile(file);
            }
        }

        static void ShowTasks(string file)
        {
            Console.WriteLine($"Tasks in file '{file}':");
            if (tasksDict.ContainsKey(file))
            {
                var tasks = tasksDict[file];
                if (tasks.Count == 0)
                {
                    Console.WriteLine("No tasks.");
                }
                else
                {
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {tasks[i]}");
                    }
                }
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
        }

        static void AddTask(string file)
        {
            Console.WriteLine("Enter the new task:");
            string newTask = Console.ReadLine();
            if (tasksDict.ContainsKey(file))
            {
                tasksDict[file].Add(newTask);
                Console.WriteLine("Task added successfully.");
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
        }

        static void MarkTaskCompleted(string file)
        {
            Console.WriteLine("Enter the number of the completed task:");
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0)
            {
                if (tasksDict.ContainsKey(file))
                {
                    var tasks = tasksDict[file];
                    if (taskNumber <= tasks.Count)
                    {
                        Console.WriteLine($"Task '{tasks[taskNumber - 1]}' marked as completed. Good job, you did it great!");
                        tasks.RemoveAt(taskNumber - 1);
                    }
                    else
                    {
                        Console.WriteLine("Invalid task number.");
                    }
                }
                else
                {
                    Console.WriteLine("File does not exist.");
                }
            }
            else
            {
                Console.WriteLine("Invalid task number.");
            }
        }

        static void FileActionsMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("File Actions:");
                Console.WriteLine("1. Create new file");
                Console.WriteLine("2. Open existing file");
                Console.WriteLine("3. View existing files");
                Console.WriteLine("4. Go back");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CreateNewFile();
                        break;
                    case "2":
                        OpenExistingFile();
                        break;
                    case "3":
                        ViewExistingFiles();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please select a valid option.");
                        break;
                }
            }
        }

        static void CreateNewFile()
        {
            Console.WriteLine("Enter the filename for the new file:");
            string fileName = Console.ReadLine();
            string newFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"{fileName}.txt");

            if (File.Exists(newFilePath))
            {
                Console.WriteLine("File already exists.");
            }
            else
            {
                File.Create(newFilePath).Close();
                tasksDict[newFilePath] = new List<string>();
                Console.WriteLine($"File '{fileName}.txt' created successfully.");
            }
        }

        static void OpenExistingFile()
        {
            Console.WriteLine("Enter the filename to open:");
            string fileName = Console.ReadLine();
            string fileToOpen = Path.Combine(Directory.GetCurrentDirectory(), $"{fileName}.txt");

            if (File.Exists(fileToOpen))
            {
                tasksDict[fileToOpen] = new List<string>(File.ReadAllLines(fileToOpen));
                Console.WriteLine($"Content of file '{fileName}.txt':");
                ShowTasks(fileToOpen);
                TaskActionsMenu(fileToOpen);
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
        }

        static void ViewExistingFiles()
        {
            Console.WriteLine("Existing files:");

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");

            if (files.Length == 0)
            {
                Console.WriteLine("No text files found.");
            }
            else
            {
                foreach (string file in files)
                {
                    Console.WriteLine(Path.GetFileName(file));
                }
            }
        }

        static void TaskActionsMenu(string file)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Task Actions:");
                Console.WriteLine("1. View tasks");
                Console.WriteLine("2. Add task");
                Console.WriteLine("3. Mark task as completed");
                Console.WriteLine("4. Go back");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ShowTasks(file);
                        break;
                    case "2":
                        AddTask(file);
                        break;
                    case "3":
                        MarkTaskCompleted(file);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please select a valid option.");
                        break;
                }
            }
        }
    }
}