using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CyberSecurityChatBotAI
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<SecurityTask> Tasks { get; set; }
        private string currentUserName = "User";
        private DispatcherTimer reminderTimer;

        // Use your ChatBot class for all chatbot logic
        private ChatBot chatBot;
        private QuizManager quizManager = new QuizManager();

        public MainWindow()
        {
            InitializeComponent();
            chatBot = new ChatBot();
            InitializeApplication();
        }

        private void InitializeApplication()
        {
            Tasks = new ObservableCollection<SecurityTask>();
            TaskListBox.ItemsSource = Tasks;
            SetPlaceholderText();

            reminderTimer = new DispatcherTimer();
            reminderTimer.Interval = TimeSpan.FromMinutes(1);
            reminderTimer.Tick += CheckReminders;
            reminderTimer.Start();

            // Use ChatBot for welcome message
            AddMessageToChat("🛡️ Cybersecurity Assistant", chatBot.GetPersonalizedGreeting());
            LoadSampleTasks();
        }

        private void LoadSampleTasks()
        {
            Tasks.Add(new SecurityTask
            {
                Id = Guid.NewGuid(),
                Title = "Enable Two-Factor Authentication",
                Description = "Set up 2FA on all important accounts (email, banking, social media)",
                ReminderDate = DateTime.Now.AddDays(1),
                IsCompleted = false,
                CreatedDate = DateTime.Now
            });

            Tasks.Add(new SecurityTask
            {
                Id = Guid.NewGuid(),
                Title = "Update Passwords",
                Description = "Review and update passwords for all accounts using strong, unique passwords",
                ReminderDate = DateTime.Now.AddDays(7),
                IsCompleted = false,
                CreatedDate = DateTime.Now
            });
        }

        private void SetPlaceholderText()
        {
            TaskTitleBox.Text = "Task title...";
            TaskTitleBox.Foreground = System.Windows.Media.Brushes.Gray;
            TaskDescriptionBox.Text = "Task description...";
            TaskDescriptionBox.Foreground = System.Windows.Media.Brushes.Gray;
        }

        // Event Handlers
        private void SendButton_Click(object sender, RoutedEventArgs e) => ProcessUserInput();

        private void MessageInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ProcessUserInput();
        }

        private void NameInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) UpdateUserName();
        }

        private void TaskTitleBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TaskTitleBox.Text == "Task title...")
            {
                TaskTitleBox.Text = "";
                TaskTitleBox.Foreground = System.Windows.Media.Brushes.White;
            }
        }

        private void TaskDescriptionBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TaskDescriptionBox.Text == "Task description...")
            {
                TaskDescriptionBox.Text = "";
                TaskDescriptionBox.Foreground = System.Windows.Media.Brushes.White;
            }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e) => AddNewTask();

        private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TaskListBox.SelectedItem is SecurityTask selectedTask)
                ShowTaskOptions(selectedTask);
        }

        // Core Logic Methods
        private void ProcessUserInput()
        {
            string userName = string.IsNullOrWhiteSpace(NameInputBox.Text) ? currentUserName : NameInputBox.Text.Trim();
            string message = MessageInputBox.Text.Trim();

            if (string.IsNullOrEmpty(message)) return;

            if (userName != currentUserName)
            {
                currentUserName = userName;
                UpdateUserName();
            }

            AddMessageToChat(userName, message);
            MessageInputBox.Text = "";

            // Detect "play quiz" command
            if (message.Equals("play quiz", StringComparison.OrdinalIgnoreCase))
            {
                Dispatcher.Invoke(() =>
                {
                    AddMessageToChat("🛡️ Assistant", "Starting the quiz! Get ready...");
                });

                // Wait 2 seconds, then open the quiz window(added this feature so the user can read the log for a few seconds before the game starts )
                Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    Dispatcher.Invoke(() =>
                    {
                        Console.WriteLine("game started");
                        var quizWindow = new QuizWindow();
                        quizWindow.Closed += (s, e) => Console.WriteLine("game ended");
                        quizWindow.ShowDialog();
                    });
                });
                return;
            }

            // Use ChatBot for response
            Task.Run(() =>
            {
                chatBot.SetUserName(currentUserName);

                string response;
                if (message.ToLower().Contains("add task"))
                    response = HandleTaskAddition(message);
                else if (message.ToLower().Contains("show tasks") || message.ToLower().Contains("list tasks"))
                    response = HandleTaskListing();
                else if (message.ToLower().Contains("complete task") || message.ToLower().Contains("finish task"))
                    response = HandleTaskCompletion(message);
                else
                    response = chatBot.ProcessMessage(message);

                Dispatcher.Invoke(() => AddMessageToChat("🛡️ Assistant", response));
            });
        }
        // Handle task addition logic
        private string HandleTaskAddition(string message)
        {
            // Example: Parse a simple "add task" command like "add task Update antivirus software"
            string title = "New Task";
            string description = "No description provided";

            // Try to extract a task title from the message
            var parts = message.Split(new[] { "add task" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0 && !string.IsNullOrWhiteSpace(parts[0]))
            {
                title = parts[0].Trim();
            }
            else if (parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]))
            {
                title = parts[1].Trim();
            }

            // Pre-fill the UI fields for user to review/edit
            Dispatcher.Invoke(() =>
            {
                TaskTitleBox.Text = title;
                TaskTitleBox.Foreground = System.Windows.Media.Brushes.White;
                TaskDescriptionBox.Text = description;
                TaskDescriptionBox.Foreground = System.Windows.Media.Brushes.White;
            });

            return "I've prepared a task for you in the task panel. You can modify the details and click 'Add Task' to save it.";
        }
        // Handle task listing logic
        private string HandleTaskListing()
        {
            if (Tasks.Count == 0)
                return "You don't have any cybersecurity tasks yet. Would you like me to suggest some important security tasks to get started?";

            int completedTasks = Tasks.Count(t => t.IsCompleted);
            int pendingTasks = Tasks.Count - completedTasks;
            return $"You have {Tasks.Count} total tasks: {completedTasks} completed and {pendingTasks} pending. Check the task panel on the right to see all your cybersecurity tasks and their details.";
        }
        // Handle task completion logic
        private string HandleTaskCompletion(string message)
        {
            var incompleteTasks = Tasks.Where(t => !t.IsCompleted).ToList();
            if (incompleteTasks.Any())
                return $"You have {incompleteTasks.Count} pending tasks. Select one in the task list to mark as complete.";
            return "Excellent! You've completed all your cybersecurity tasks. Keep up the great work!";
        }
        // Add a new task to the list
        private void AddNewTask()
        {
            string title = TaskTitleBox.Text.Trim();
            string description = TaskDescriptionBox.Text.Trim();

            if (string.IsNullOrEmpty(title) || title == "Task title...")
            {
                MessageBox.Show("Please enter a task title.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(description) || description == "Task description...")
                description = "No description provided";

            var newTask = new SecurityTask
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                ReminderDate = ReminderDatePicker.SelectedDate ?? DateTime.Now.AddDays(7),
                IsCompleted = false,
                CreatedDate = DateTime.Now
            };

            Tasks.Add(newTask);
            SetPlaceholderText();
            ReminderDatePicker.SelectedDate = null;
            AddMessageToChat("🛡️ Assistant", $"✅ Task '{title}' has been added successfully! I'll remind you on {newTask.ReminderDate:MM/dd/yyyy}.");
        }
        // Show task options when a task is selected
        private void ShowTaskOptions(SecurityTask task)
        {
            var result = MessageBox.Show(
                $"Task: {task.Title}\n\nDescription: {task.Description}\n\nDue: {task.ReminderDate:MM/dd/yyyy}\n\nWhat would you like to do?",
                "Task Options",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question,
                MessageBoxResult.Cancel,
                MessageBoxOptions.None);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    task.IsCompleted = true;
                    task.CompletedDate = DateTime.Now;
                    AddMessageToChat("🛡️ Assistant", $"🎉 Great job! You've completed '{task.Title}'.");
                    break;
                case MessageBoxResult.No:
                    if (MessageBox.Show("Are you sure you want to delete this task?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Tasks.Remove(task);
                        AddMessageToChat("🛡️ Assistant", "🗑️ Task has been deleted successfully.");
                    }
                    break;
            }
        }
        // Check for reminders every minute
        private void CheckReminders(object sender, EventArgs e)
        {
            var dueTasks = Tasks.Where(t => !t.IsCompleted && t.ReminderDate.Date <= DateTime.Now.Date && !t.ReminderShown).ToList();
            foreach (var task in dueTasks)
            {
                AddMessageToChat("⏰ Reminder", $"Don't forget: '{task.Title}' is due today!");
                task.ReminderShown = true;
            }
        }
        // Update user name and welcome message
        private void UpdateUserName()
        {
            if (!string.IsNullOrWhiteSpace(NameInputBox.Text))
            {
                currentUserName = NameInputBox.Text.Trim();
                UserNameText.Text = $"Welcome, {currentUserName}!";
                chatBot.SetUserName(currentUserName);
            }
        }
        // Add a message to the chat display
        private void AddMessageToChat(string sender, string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm");
            string formattedMessage = $"[{timestamp}] {sender}: {message}\n\n";
            ChatDisplay.Text += formattedMessage;
            ChatScrollViewer.ScrollToEnd();
        }
        // Event Handlers for Quiz
        private void ShowQuizButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("game started");
            var quizWindow = new QuizWindow();
            quizWindow.ShowDialog();
        }
        private void QuizSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement quiz answer submission logic
        }

        private void QuizNextButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement logic to show the next quiz question
        }

        private void QuizStartButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement logic to start the quiz
        }
    }

    // Task Model Class (unchanged)
    public class SecurityTask : INotifyPropertyChanged
    {
        private bool _isCompleted;
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReminderDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool ReminderShown { get; set; }
        public bool IsCompleted
        {
            get => _isCompleted;
            set { _isCompleted = value; OnPropertyChanged(nameof(IsCompleted)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
