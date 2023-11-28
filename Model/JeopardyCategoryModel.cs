using System.ComponentModel;
using Jeopardy.Configuration;
using Jeopardy.Helper;
using Jeopardy.Validation;

namespace Jeopardy.Model;

public class JeopardyCategoryModel : ModelBase
{
    private bool _isNameRevealed;

    public JeopardyCategoryModel(Category category, int pointsFactor)
    {
        category.Name.AssertNotNull(nameof(category.Name));
        category.Questions.AssertNotNull(nameof(category.Questions));
        
        Name = category.Name;
        IsNameRevealed = false;

        var defaultPoints = pointsFactor;
        foreach (var question in category.Questions)
        {
            Questions.Add(new JeopardyQuestionModel(question, defaultPoints));
            defaultPoints += pointsFactor;
        }

        Questions.PropertyChanged += QuestionsOnPropertyChanged;
    }

    public string Name { get; }

    public bool IsNameRevealed { get => _isNameRevealed; private set => SetField(ref _isNameRevealed, value); }

    public ObservableCollectionWithItemUpdates<JeopardyQuestionModel> Questions { get; } = new();

    public void RevealName() => IsNameRevealed = true;

    private void QuestionsOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Questions));
    }
}