using Jeopardy.Configuration;
using Jeopardy.Helper;
using Jeopardy.Validation;

namespace Jeopardy.Model;

public class JeopardyGameModel : ModelBase
{
    public JeopardyGameModel(JeopardyConfiguration configuration, JeopardyQuestions questions)
    {
        questions.Categories.AssertNotNull(nameof(questions.Categories));
        configuration.Teams.AssertNotNull(nameof(configuration.Teams));
        var pointsFactor = configuration.PointsFactor ?? JeopardyConfiguration.DefaultPointsFactor;
        
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var category in questions.Categories)
        {
            Categories.Add(new JeopardyCategoryModel(category, pointsFactor));
        }

        foreach (var team in configuration.Teams)
        {
            Teams.Add(new JeopardyTeamModel(configuration, team));
        }
        
        Categories.PropertyChanged += (_, _) => OnPropertyChanged(nameof(Categories));
        Teams.PropertyChanged += (_, _) => OnPropertyChanged(nameof(Teams));
    }
    
    public ObservableCollectionWithItemUpdates<JeopardyCategoryModel> Categories { get; } = new();
    public ObservableCollectionWithItemUpdates<JeopardyTeamModel> Teams { get; } = new();
}