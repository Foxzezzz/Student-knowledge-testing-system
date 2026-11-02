using KnowledgeTesting.Core;
using Xunit;

namespace KnowledgeTesting.Tests;

public class SessionTests
{
    private sealed class OneQuestionBank : IQuestionBank
    {
        public IReadOnlyList<Question> GetQuestions() => new[]
        {
            new Question("one", "Сколько будет два плюс два?", new[] { "4", "5" }, 0)
        };
    }

    [Fact]
    public void CorrectAnswer_CompletesSessionAndCalculatesScore()
    {
        var session = new TestSession(new OneQuestionBank());
        var progress = session.SubmitAnswer(0);
        Assert.True(progress.IsComplete);
        Assert.Equal(1, progress.Answered);
        Assert.Equal(new TestResult(1, 1, 100), session.GetResult());
    }

    [Fact]
    public void WrongAnswer_IsCountedWithoutCompletingCorrectly()
    {
        var session = new TestSession(new OneQuestionBank());
        session.SubmitAnswer(1);
        Assert.Equal(new TestResult(0, 1, 0), session.GetResult());
    }

    [Fact]
    public void InvalidOption_DoesNotAdvanceProgress()
    {
        var session = new TestSession(new OneQuestionBank());
        Assert.Throws<ArgumentOutOfRangeException>(() => session.SubmitAnswer(2));
        Assert.Equal(0, session.Answered);
        Assert.False(session.IsComplete);
    }

    [Fact]
    public void ResultBeforeCompletion_IsUnavailable()
    {
        var session = new TestSession(new OneQuestionBank());
        Assert.Throws<InvalidOperationException>(() => session.GetResult());
    }

    [Fact]
    public void CompletedSession_RejectsFurtherAnswers()
    {
        var session = new TestSession(new OneQuestionBank());
        session.SubmitAnswer(0);
        Assert.Null(session.CurrentQuestion);
        Assert.Throws<InvalidOperationException>(() => session.SubmitAnswer(0));
    }

    [Fact]
    public void Question_RejectsMissingText()
    {
        Assert.Throws<ArgumentException>(() => new Question("one", " ", new[] { "да", "нет" }, 0));
    }
}
