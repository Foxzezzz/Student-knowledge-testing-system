namespace KnowledgeTesting.Core;

public interface IQuestionBank
{
    IReadOnlyList<Question> GetQuestions();
}
