using System.Windows;
using System.Windows.Controls;

namespace CyberSecurityChatBotAI
{
    public partial class QuizWindow : Window
    {
        private QuizManager quizManager = new QuizManager();

        public QuizWindow()
        {
            InitializeComponent();
        }
        // action for the QuizStartButton click event
        private void QuizStartButton_Click(object sender, RoutedEventArgs e)
        {
            quizManager.Reset();
            QuizStartButton.Visibility = Visibility.Collapsed;
            QuizSubmitButton.Visibility = Visibility.Visible;
            QuizNextButton.Visibility = Visibility.Collapsed;
            QuizFeedbackText.Text = "";
            QuizScoreText.Text = "";
            ShowCurrentQuizQuestion();
        }
        //
        private void ShowCurrentQuizQuestion()
        {
            var question = quizManager.GetCurrentQuestion();
            if (question == null)
                return;

            QuizQuestionText.Text = $"Q{quizManager.CurrentQuestionNumber}: {question.QuestionText}";
            QuizOptionsList.ItemsSource = question.Options;
            QuizOptionsList.SelectedIndex = -1;
            QuizFeedbackText.Text = "";
            QuizScoreText.Text = $"Score: {quizManager.Score}/{quizManager.TotalQuestions}";
        }
        // quiz submit button click event
        private void QuizSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (QuizOptionsList.SelectedIndex == -1)
            {
                QuizFeedbackText.Text = "Please select an answer.";
                return;
            }

            bool correct = quizManager.CheckAnswer(QuizOptionsList.SelectedIndex, out string explanation);
            QuizFeedbackText.Text = (correct ? "Correct! " : "Incorrect. ") + explanation;
            QuizSubmitButton.Visibility = Visibility.Collapsed;
            QuizNextButton.Visibility = Visibility.Visible;
            QuizScoreText.Text = $"Score: {quizManager.Score}/{quizManager.TotalQuestions}";
        }

        private void QuizNextButton_Click(object sender, RoutedEventArgs e)
        {
            if (quizManager.NextQuestion())
            {
                ShowCurrentQuizQuestion();
                QuizSubmitButton.Visibility = Visibility.Visible;
                QuizNextButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                QuizQuestionText.Text = "Quiz Complete!";
                QuizOptionsList.ItemsSource = null;
                QuizSubmitButton.Visibility = Visibility.Collapsed;
                QuizNextButton.Visibility = Visibility.Collapsed;
                QuizFeedbackText.Text = quizManager.Score >= 8
                    ? "Great job! You're a cybersecurity pro!"
                    : "Keep learning to stay safe online!";
                QuizScoreText.Text = $"Final Score: {quizManager.Score}/{quizManager.TotalQuestions}";
                QuizStartButton.Visibility = Visibility.Visible;
            }
        }

        private void QuizExitButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("game ended");
            this.Close();
        }
    }
}
