using System.Windows;
using KnowledgeTesting.Core;

namespace KnowledgeTesting.Desktop;

public partial class MainWindow : Window
{
    private TestSession session = new(new DemoQuestionBank());

    public MainWindow()
    {
        InitializeComponent();
        ShowQuestion();
    }

    private void StartNewTest()
    {
        session = new TestSession(new DemoQuestionBank());
        AnswerButton.IsEnabled = true;
        OptionsList.IsEnabled = true;
        ResultText.Text = "";
        ShowQuestion();
    }

    private void ShowQuestion()
    {
        var question = session.CurrentQuestion;
        if (question is null)
            return;
        ProgressText.Text = $"Вопрос {session.Answered + 1} из {session.Total}";
        QuestionText.Text = question.Text;
        OptionsList.ItemsSource = question.Options;
        OptionsList.SelectedIndex = -1;
    }

    private void AnswerButton_Click(object sender, RoutedEventArgs e)
    {
        if (OptionsList.SelectedIndex < 0)
        {
            MessageBox.Show("Выберите вариант ответа.", "Проверка ответа", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var progress = session.SubmitAnswer(OptionsList.SelectedIndex);
        if (!progress.IsComplete)
        {
            ShowQuestion();
            return;
        }

        var result = session.GetResult();
        ProgressText.Text = "Тест завершён";
        QuestionText.Text = "Результат готов";
        OptionsList.ItemsSource = null;
        OptionsList.IsEnabled = false;
        AnswerButton.IsEnabled = false;
        ResultText.Text = $"Верных ответов: {result.Correct} из {result.Total}. Результат: {result.Percentage:F1}%.";
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e) => StartNewTest();
}
