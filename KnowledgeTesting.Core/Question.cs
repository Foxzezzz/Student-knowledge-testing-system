namespace KnowledgeTesting.Core;

public sealed class Question
{
    public Question(string id, string text, IEnumerable<string> options, int correctOptionIndex)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Укажите идентификатор вопроса.", nameof(id));
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Укажите текст вопроса.", nameof(text));
        ArgumentNullException.ThrowIfNull(options);
        var copy = options.ToArray();
        if (copy.Length < 2)
            throw new ArgumentException("Нужны как минимум два варианта ответа.", nameof(options));
        if (copy.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Варианты ответа не должны быть пустыми.", nameof(options));
        if (correctOptionIndex < 0 || correctOptionIndex >= copy.Length)
            throw new ArgumentOutOfRangeException(nameof(correctOptionIndex));

        Id = id.Trim();
        Text = text.Trim();
        Options = Array.AsReadOnly(copy);
        CorrectOptionIndex = correctOptionIndex;
    }

    public string Id { get; }
    public string Text { get; }
    public IReadOnlyList<string> Options { get; }
    public int CorrectOptionIndex { get; }
}
