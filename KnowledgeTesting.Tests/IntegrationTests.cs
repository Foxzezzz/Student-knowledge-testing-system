using KnowledgeTesting.Core;
using Xunit;

namespace KnowledgeTesting.Tests;

public class IntegrationTests
{
    [Fact]
    public void DemoBank_CanBeUsedForACompleteTest()
    {
        var session = new TestSession(new DemoQuestionBank());
        Assert.Equal(5, session.Total);
        while (session.CurrentQuestion is { } question)
            session.SubmitAnswer(question.CorrectOptionIndex);
        Assert.Equal(new TestResult(5, 5, 100), session.GetResult());
    }

    [Fact]
    public void DemoBank_InvalidAnswerIsRejectedWithoutAdvancing()
    {
        var session = new TestSession(new DemoQuestionBank());
        Assert.Throws<ArgumentOutOfRangeException>(() => session.SubmitAnswer(-1));
        Assert.Equal(0, session.Answered);
    }
}
