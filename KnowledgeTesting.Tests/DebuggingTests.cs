using KnowledgeTesting.Core;
using Xunit;

namespace KnowledgeTesting.Tests;

public class DebuggingTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Question_RejectsBlankAnswerOption(string? option)
    {
        Assert.Throws<ArgumentException>(() => new Question("q1", "Вопрос?", new[] { "Ответ", option! }, 0));
    }

    [Fact]
    public void Session_RejectsDuplicateQuestionIds()
    {
        Assert.Throws<ArgumentException>(() => new TestSession(new DuplicateQuestionBank()));
    }

    private sealed class DuplicateQuestionBank : IQuestionBank
    {
        public IReadOnlyList<Question> GetQuestions() => new[]
        {
            new Question("q1", "Первый вопрос?", new[] { "да", "нет" }, 0),
            new Question("Q1", "Второй вопрос?", new[] { "да", "нет" }, 1)
        };
    }
}
