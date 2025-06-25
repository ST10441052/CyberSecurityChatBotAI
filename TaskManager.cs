using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CyberSecurityChatBotAI_poe
{
    class TaskManager
    {
        private List<CyberTask> tasks;
        private readonly string tasksFilePath = "CyberTasks.json";

        public TaskManager()
        {
            tasks = new List<CyberTask>();
            LoadTasks();
        }

        public void AddTask(string title, string description, DateTime? reminderDate = null, TaskPriority priority = TaskPriority.Medium)
        {
            var task = new CyberTask(title, description, reminderDate)
            {
                Priority = priority
            };
            tasks.Add(task);
            SaveTasks();
        }

        public void AddTask(CyberTask task)
        {
            if (task != null)
            {
                tasks.Add(task);
                SaveTasks();
            }
        }

        public List<CyberTask> GetAllTasks()
        {
            return new List<CyberTask>(tasks);
        }

        public List<CyberTask> GetPendingTasks()
        {
            return tasks.Where(t => !t.IsCompleted).ToList();
        }

        public List<CyberTask> GetOverdueTasks()
        {
            return tasks.Where(t => t.IsOverdue()).ToList();
        }

        public List<CyberTask> GetTasksDueToday()
        {
            var today = DateTime.Today;
            return tasks.Where(t => t.ReminderDate.HasValue &&
                                   t.ReminderDate.Value.Date == today &&
                                   !t.IsCompleted).ToList();
        }

        public List<CyberTask> GetTasksDueInDays(int days)
        {
            var targetDate = DateTime.Today.AddDays(days);
            return tasks.Where(t => t.ReminderDate.HasValue &&
                                   t.ReminderDate.Value.Date <= targetDate &&
                                   !t.IsCompleted).ToList();
        }

        public bool CompleteTask(string taskId)
        {
            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.IsCompleted = true;
                SaveTasks();
                return true;
            }
            return false;
        }

        public bool DeleteTask(string taskId)
        {
            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                tasks.Remove(task);
                SaveTasks();
                return true;
            }
            return false;
        }

        public CyberTask GetTask(string taskId)
        {
            return tasks.FirstOrDefault(t => t.Id == taskId);
        }

        public void UpdateTask(string taskId, string title = null, string description = null,
                              DateTime? reminderDate = null, TaskPriority? priority = null)
        {
            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                if (!string.IsNullOrEmpty(title)) task.Title = title;
                if (!string.IsNullOrEmpty(description)) task.Description = description;
                if (reminderDate.HasValue) task.ReminderDate = reminderDate;
                if (priority.HasValue) task.Priority = priority.Value;
                SaveTasks();
            }
        }

        public int GetTaskCount()
        {
            return tasks.Count;
        }

        public int GetPendingTaskCount()
        {
            return tasks.Count(t => !t.IsCompleted);
        }

        public int GetOverdueTaskCount()
        {
            return tasks.Count(t => t.IsOverdue());
        }

        // Get recommended cybersecurity tasks
        public List<CyberTask> GetRecommendedTasks()
        {
            var recommendedTasks = new List<CyberTask>
            {
                new CyberTask("Enable Two-Factor Authentication",
                             "Set up 2FA on all important accounts (email, banking, social media)",
                             DateTime.Now.AddDays(1)) { Priority = TaskPriority.High },

                new CyberTask("Update All Passwords",
                             "Change weak passwords and ensure unique passwords for each account",
                             DateTime.Now.AddDays(3)) { Priority = TaskPriority.High },

                new CyberTask("Review Privacy Settings",
                             "Check and update privacy settings on social media and online accounts",
                             DateTime.Now.AddDays(7)) { Priority = TaskPriority.Medium },

                new CyberTask("Install Security Updates",
                             "Update operating system, browsers, and security software",
                             DateTime.Now.AddDays(1)) { Priority = TaskPriority.Critical },

                new CyberTask("Backup Important Data",
                             "Create secure backups of important files and documents",
                             DateTime.Now.AddDays(14)) { Priority = TaskPriority.Medium },

                new CyberTask("Security Awareness Training",
                             "Complete cybersecurity awareness training or read security guidelines",
                             DateTime.Now.AddDays(30)) { Priority = TaskPriority.Low },

                new CyberTask("Review Bank Statements",
                             "Check for unauthorized transactions and suspicious activity",
                             DateTime.Now.AddDays(7)) { Priority = TaskPriority.Medium },

                new CyberTask("Configure Firewall",
                             "Ensure firewall is enabled and properly configured",
                             DateTime.Now.AddDays(2)) { Priority = TaskPriority.High }
            };

            return recommendedTasks;
        }

        private void SaveTasks()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                string jsonString = JsonSerializer.Serialize(tasks, options);
                File.WriteAllText(tasksFilePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tasks: {ex.Message}");
            }
        }

        private void LoadTasks()
        {
            try
            {
                if (File.Exists(tasksFilePath))
                {
                    string jsonString = File.ReadAllText(tasksFilePath);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    tasks = JsonSerializer.Deserialize<List<CyberTask>>(jsonString, options) ?? new List<CyberTask>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tasks: {ex.Message}");
                tasks = new List<CyberTask>();
            }
        }

        public string GetTaskSummary()
        {
            var total = GetTaskCount();
            var pending = GetPendingTaskCount();
            var overdue = GetOverdueTaskCount();
            var dueToday = GetTasksDueToday().Count;

            return $"Tasks: {total} total, {pending} pending, {overdue} overdue, {dueToday} due today";
        }

        public void ClearAllTasks()
        {
            tasks.Clear();
            SaveTasks();
        }

        public void ClearCompletedTasks()
        {
            tasks.RemoveAll(t => t.IsCompleted);
            SaveTasks();
        }
    }
}