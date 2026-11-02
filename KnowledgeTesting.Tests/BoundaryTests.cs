using KnowledgeTesting.Core;
using Xunit;

namespace KnowledgeTesting.Tests;

public class BoundaryTests
{
    [Fact]
    public void Session_RejectsNullBank()
    {
        Assert.Throws<ArgumentNullException>(() => new TestSession(null!));
    }

    [Fact]
    public void Session_RejectsEmptyBank()
    {
        Assert.Throws<ArgumentException>(() => new TestSession(new MutableBank()));
    }

    [Fact]
    public void Question_CopiesOptionsAndDoesNotExposeMutableList()
    {
        string[] options = ["первый", "второй"];
        var question = new Question("q", "Вопрос?", options, 0);
        options[0] = "изменён";
        Assert.Equal("первый", question.Options[0]);
        Assert.Throws<NotSupportedException>(() => ((IList<string>)question.Options)[0] = "изменён");
    }

    [Fact]
    public void Session_KeepsSnapshotOfBank()
    {
        var bank = new MutableBank();
        bank.Items.Add(new Question("q", "Вопрос?", new[] { "да", "нет" }, 0));
        var session = new TestSession(bank);
        bank.Items.Clear();
        Assert.Equal(1, session.Total);
        Assert.NotNull(session.CurrentQuestion);
    }

    [Fact]
    public void Score_UsesFractionOfCorrectAnswers()
    {
        var bank = new MutableBank();
        bank.Items.Add(new Question("q1", "Первый?", new[] { "да", "нет" }, 0));
        bank.Items.Add(new Question("q2", "Второй?", new[] { "да", "нет" }, 0));
        bank.Items.Add(new Question("q3", "Третий?", new[] { "да", "нет" }, 0));
        var session = new TestSession(bank);
        session.SubmitAnswer(0);
        session.SubmitAnswer(1);
        session.SubmitAnswer(0);
        var result = session.GetResult();
        Assert.Equal(2, result.Correct);
        Assert.Equal(3, result.Total);
        Assert.Equal(100.0 * 2 / 3, result.Percentage, 8);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(2)]
    public void Question_RejectsCorrectIndexOutsideOptions(int index)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Question("q", "Вопрос?", new[] { "да", "нет" }, index));
    }

    private sealed class MutableBank : IQuestionBank
    {
        public List<Question> Items { get; } = [];
        public IReadOnlyList<Question> GetQuestions() => Items;
    }
}
