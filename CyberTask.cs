using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSecurityChatBotAI_poe
{
    class CyberTask
    {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? ReminderDate { get; set; }
            public bool IsCompleted { get; set; }
            public TaskPriority Priority { get; set; }

            public CyberTask()
            {
                Id = Guid.NewGuid().ToString();
                CreatedDate = DateTime.Now;
                IsCompleted = false;
                Priority = TaskPriority.Medium;
            }

            public CyberTask(string title, string description, DateTime? reminderDate = null)
            {
                Id = Guid.NewGuid().ToString();
                Title = title;
                Description = description;
                ReminderDate = reminderDate;
                CreatedDate = DateTime.Now;
                IsCompleted = false;
                Priority = TaskPriority.Medium;
            }

            public bool IsOverdue()
            {
                return ReminderDate.HasValue && ReminderDate.Value < DateTime.Now && !IsCompleted;
            }

            public int DaysUntilDue()
            {
                if (!ReminderDate.HasValue) return int.MaxValue;
                return (ReminderDate.Value - DateTime.Now).Days;
            }

            public override string ToString()
            {
                string status = IsCompleted ? "✓" : "○";
                string reminder = ReminderDate.HasValue ? $" (Due: {ReminderDate.Value:MM/dd/yyyy})" : "";
                return $"{status} {Title}{reminder}";
            }
        }

        public enum TaskPriority
        {
            Low,
            Medium,
            High,
            Critical
        }
    }

