namespace KnowledgeTesting.Core;

public sealed class DemoQuestionBank : IQuestionBank
{
    public IReadOnlyList<Question> GetQuestions() => Array.AsReadOnly(new[]
    {
        new Question("q1", "Что такое C#?", new[] { "Язык программирования", "Система управления базами данных", "Операционная система" }, 0),
        new Question("q2", "Какой тип хранит целые числа в C#?", new[] { "string", "int", "bool" }, 1),
        new Question("q3", "Какое ключевое слово объявляет класс?", new[] { "class", "return", "using" }, 0),
        new Question("q4", "Какое значение относится к типу bool?", new[] { "3.14", "true", "текст" }, 1),
        new Question("q5", "Что делает оператор return?", new[] { "Объявляет переменную", "Возвращает управление и при необходимости значение", "Создаёт файл" }, 1)
    });
}
