using System.Diagnostics;

namespace KnowledgeTesting.Core;

public sealed class TestSession
{
    private readonly Question[] questions;
    private int answered;
    private int correct;

    public TestSession(IQuestionBank bank)
    {
        ArgumentNullException.ThrowIfNull(bank);
        var source = bank.GetQuestions() ?? throw new ArgumentException("Банк не вернул список вопросов.", nameof(bank));
        questions = source.ToArray();
        if (questions.Length == 0 || questions.Any(question => question is null))
            throw new ArgumentException("Банк должен содержать вопросы.", nameof(bank));
        Trace.TraceInformation("Test started: questionCount={0}", questions.Length);
    }

    public int Total => questions.Length;
    public int Answered => answered;
    public bool IsComplete => answered == Total;
    public Question? CurrentQuestion => IsComplete ? null : questions[answered];

    public AnswerProgress SubmitAnswer(int optionIndex)
    {
        if (IsComplete)
            throw new InvalidOperationException("Тест уже завершён.");
        var question = questions[answered];
        if (optionIndex < 0 || optionIndex >= question.Options.Count)
            throw new ArgumentOutOfRangeException(nameof(optionIndex), "Выберите существующий вариант ответа.");
        var isCorrect = optionIndex == question.CorrectOptionIndex;
        if (isCorrect)
            correct++;
        answered++;
        Trace.TraceInformation("Answer accepted: questionId={0}, answered={1}, correct={2}", question.Id, answered, isCorrect);
        return new AnswerProgress(answered, IsComplete);
    }

    public TestResult GetResult()
    {
        if (!IsComplete)
            throw new InvalidOperationException("Результат доступен после ответа на все вопросы.");
        return new TestResult(correct, Total, 100.0 * correct / Total);
    }
}
